using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement2D : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = 8f;
    public float slopeMaxThreshold = 50f;
    public float jumpDuration = 1f;
    public int maxAllowedJumps = 3;
    public AnimationCurve jumpCurve;

    public Raycaster2D raycaster;

    bool isGrounded; // always starts false, we're mid-air for the first frame

    // temp variables for handling jump
    bool isJumping;
    float timeSinceJumped;
    int currentlyRemainingJumps;

    void Start()
    {
        currentlyRemainingJumps = maxAllowedJumps - 1;
    }
    
    void Update()
    {
        HorizontalUpdate();
        VerticalUpdate();
    }

    void HorizontalUpdate()
    {
        // getting player input
        float moveDirection = Input.GetAxis("Horizontal");

        // throw rays to the left/right and move it allowed
        Move(Vector2.right * moveDirection * speed * Time.deltaTime); // same as new Vector2(moveDirection * 1, moveDirection * 0);
    }

    void VerticalUpdate()
    {
        // Listen to user input here
        if (Input.GetKeyDown(KeyCode.Space)) Jump();

        // Prepare to store the collision detection result into this variable
        RaycastHit2D result;

        // Jump update
        if (isJumping)
        {
            // Compare character height on the jump curve at two consecutive frames
            float previousHeight = jumpCurve.Evaluate(timeSinceJumped);
            timeSinceJumped += Time.deltaTime;
            float currentWantedHeight = jumpCurve.Evaluate(timeSinceJumped);

            // Vertical movement is the difference of altitude between these two frames
            result = Move(Vector2.up * (currentWantedHeight-previousHeight));

            // End jump when the end of the curve is reached
            if (timeSinceJumped > jumpCurve.keys[jumpCurve.keys.Length-1].time)
                isJumping = false;
        }
        // Gravity update
        else
        {
            // We literally just have to move down
            result = Move(Vector2.down * gravity * Time.deltaTime);
        }
        
        // Update grounded state
        isGrounded = (result.collider != null);
        if (isGrounded) currentlyRemainingJumps = maxAllowedJumps;
    }

    // Trigger called only upon pressing the key.
    public void Jump()
    {
        // Locking the use of this function if we ran out of jumps
        if (currentlyRemainingJumps == 0) return;

        // the fact that everything get reset here makes the Update adopt a jump behaviour
        isJumping = true;
        timeSinceJumped = 0f;
        currentlyRemainingJumps--;
    }

    // Function that should be used instead of Translate(), so it auto-aborts the movement if it would collide
    RaycastHit2D Move(Vector2 moveDir)
    {
        // we assume it's only to be used with one-dimensional movement (only X or only Y)
        RayDirection rayDir = RayDirection.Down;
        if (moveDir.x > 0) rayDir = RayDirection.Right;
        if (moveDir.x < 0) rayDir = RayDirection.Left;
        if (moveDir.y > 0) rayDir = RayDirection.Up;
        if (moveDir.y < 0) rayDir = RayDirection.Down;

        // throw rays to see if there's a collision
        RaycastHit2D result = raycaster.ThrowRays(rayDir, moveDir.magnitude);

        // if no collision at all, just performed the movement normally
        if (result.collider == null) transform.Translate(moveDir);
        // in case of a collision, perform what we can (get as close as possible) before aborting
        else
        {
            // distance to the wall is what we can travel safely
            float hitDistance = result.distance;
            transform.Translate(moveDir.normalized * hitDistance);

            // BONUS : SLOPE HANDLING //

            // was there a slope? if so, if the angle allows it, move up so we stay above the ground
            if (moveDir.y == 0) // we will not process vertical movement
            {
                // calculate slope angle and only move if it's low enough
                float slopeAngle = Mathf.Abs(90f - Vector2.Angle(moveDir.normalized, result.normal));
                if (slopeAngle < slopeMaxThreshold)
                {
                    Debug.Log(slopeAngle);
                    // calc the remaining horizontal movement to make
                    float remainingMovement = Mathf.Abs(moveDir.x) - hitDistance;
                    // calc the vertical displacement it would represent if it was performed
                    float correspondingVerticalMovement = Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * remainingMovement;
                    // try to perform vertical movement
                    if (Move(Vector2.up * correspondingVerticalMovement).collider == null)
                    {
                        // if this vertical movement didn't hit the ceiling, we can perform the remaining horizontal movement
                        transform.Translate(moveDir.normalized * remainingMovement);
                    }
                }
            }
        }

        return result;
    }
}
