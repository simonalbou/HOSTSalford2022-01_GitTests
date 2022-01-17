using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement2D : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = 8f;

    public Raycaster2D raycaster;

    bool shouldBeDrawingGizmos;
    Vector3 gizmoStart;
    float rayLength;


    void OnDrawGizmos()
    {
        if (!shouldBeDrawingGizmos) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(gizmoStart, gizmoStart + Vector3.right * rayLength);
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
        Move(Vector2.down * gravity * Time.deltaTime);
    }

    void Move(Vector2 moveDir)
    {
        RayDirection rayDir = RayDirection.Down;
        if (moveDir.x > 0) rayDir = RayDirection.Right;
        if (moveDir.x < 0) rayDir = RayDirection.Left;
        if (moveDir.y > 0) rayDir = RayDirection.Up;
        if (moveDir.y < 0) rayDir = RayDirection.Down;

        RaycastHit2D result = raycaster.ThrowRays(rayDir, moveDir.magnitude);

        if (result.collider == null) transform.Translate(moveDir);
        else
        {
            float hitDistance = result.distance;
            Debug.Log(result.distance);
            transform.Translate(moveDir.normalized * hitDistance);
        }
    }
}
