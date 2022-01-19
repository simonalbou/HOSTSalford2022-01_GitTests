using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterMovement2D : MonoBehaviour
{
    [Header("Global movement stats")]
    public float speed = 5f;
    public float gravity = 8f;
    public float slopeMaxThreshold = 50f;

    [Header("Jump Parameters")]
    public int maxAllowedJumps = 3;
    public AnimationCurve jumpCurve;

    [Header("References")]
    public Raycaster2D raycaster;
    public Animator selfAnimator;
    public Transform graphicsTransform;

    [Header("Events")]
    public UnityEvent onJump;
    public UnityEvent onLanding;

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
    
        selfAnimator.SetFloat("CharacterSpeed", Mathf.Abs(moveDirection));

        // scale the graphics transform along the X axis to +1 or -1
        if (moveDirection != 0)
        {
            float x = Mathf.Abs(graphicsTransform.localScale.x) * Mathf.Sign(moveDirection);
            float y = graphicsTransform.localScale.y;
            float z = graphicsTransform.localScale.z;

            graphicsTransform.localScale = new Vector3(x, y, z);
        }
    }

    void VerticalUpdate()
    {
        // Listen to user input here
        if (Input.GetKeyDown(KeyCode.Space)) Jump();

        // Prepare to store the collision detection result into this variable
        RaycastHit2D result;

        // Is the player moving up or down here?
        bool playerIsMovingDown = true;

        // Jump update
        if (isJumping)
        {
            // Compare character height on the jump curve at two consecutive frames
            float previousHeight = jumpCurve.Evaluate(timeSinceJumped);
            timeSinceJumped += Time.deltaTime;
            float currentWantedHeight = jumpCurve.Evaluate(timeSinceJumped);

            // Vertical movement is the difference of altitude between these two frames
            result = Move(Vector2.up * (currentWantedHeight-previousHeight));

            // If it turns out the player is moving up, store this info so we can't get considered as "grounded" this frame
            if (currentWantedHeight - previousHeight > 0)
                playerIsMovingDown = false;

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
        // Extra boolean helps not triggering "grounded" upon hitting ceiling
        isGrounded = (result.collider != null && playerIsMovingDown);
        if (isGrounded)
        {
            currentlyRemainingJumps = maxAllowedJumps;
            isJumping = false;
        }
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

        // player just started a jump
        onJump.Invoke();
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
            // was there a slope? if so, if the angle allows it, move up so we stay above the ground
            if (moveDir.y == 0) // we will not process vertical movement
            {
                // store the horizontal distance to travel here, for the sake of clarity
                float horizontalDistance = Mathf.Abs(moveDir.x);

                // calculate slope angle and only move if it's low enough
                float slopeAngle = Mathf.Abs(90f - Vector2.Angle(moveDir.normalized, result.normal));
                if (slopeAngle < slopeMaxThreshold)
                {
                    // calc the vertical displacement it would represent if it was performed
                    float verticalDistance = Mathf.Abs(Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * horizontalDistance);
                    // try to perform vertical movement
                    RaycastHit2D resultOfVertical = Move(Vector2.up * verticalDistance);
                    // if this vertical movement didn't hit the ceiling, we can perform the remaining horizontal movement
                    if (resultOfVertical.collider == null)
                    {
                        transform.Translate(moveDir.normalized * horizontalDistance);
                    }
                }
            }

            // Hit distance (ie. distance to the wall) is what we can travel safely
            // EDIT : this is not compatible with handling slopes! It sends the ray origin INSIDE colliders.
            // A viable fix would be subtracting an infinitesimal offset to the result. 
            
            //float hitDistance = result.distance;
            //transform.Translate(moveDir.normalized * hitDistance);
        }

        return result;
    }
}
