using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RayDirection { Left, Right, Up, Down }

public class Raycaster2D : MonoBehaviour
{
    public BoxCollider2D selfCollider; // we'll need this reference to calculate ray origins
    
    [Range(2, 8)] // makes the thing into a slider in Unity's inspector
    public int accuracyLevel;

    [Tooltip("Offset added to the raycast so it doesn't collide with itself")]
    public float skinWidth = 0.02f;

    // debug only
    public Transform rayTransform;

    void Update()
    {
        // debug version for testing rays
        if (Input.GetKeyDown(KeyCode.R))
        {
            // editor-only, just like Debug.Log, but instead of printing a value we draw something on the screen
            Debug.DrawRay(rayTransform.position, rayTransform.up, Color.yellow, 2f);
            
            // actual raycast that stores the result in its RaycastHit2D return value
            RaycastHit2D result = Physics2D.Raycast(rayTransform.position, rayTransform.up, 1.0f);

            // checking if result.collider exists ("is not equal to null"), which would mean we hit something
            if (result.collider != null)
            {
                Debug.Log("we hit "+result.collider.name);
            }
        }       
    }

    public RaycastHit2D ThrowRays(RayDirection rayDirection, float rayDistance)
    {
        // This variable will hold the result. By default, it's all empty.
        RaycastHit2D result = new RaycastHit2D();

        // This is what we're gonna do in the next session:
        // step 1 : knowing which corners we actually need
        // step 2 : calculate the position of said corners

        Vector3 startCorner, endCorner;
        if (rayDirection == RayDirection.Left)
        {
            startCorner = LocateCorner(RayDirection.Left, RayDirection.Up);
            endCorner = LocateCorner(RayDirection.Left, RayDirection.Down);
        }
        else if (rayDirection == RayDirection.Right)
        {
            startCorner = LocateCorner(RayDirection.Right, RayDirection.Up);
            endCorner = LocateCorner(RayDirection.Right, RayDirection.Down);
        }
        else if (rayDirection == RayDirection.Up)
        {
            startCorner = LocateCorner(RayDirection.Left, RayDirection.Up);
            endCorner = LocateCorner(RayDirection.Right, RayDirection.Up);
        }
        else //if (rayDirection == RayDirection.Down)
        {
            startCorner = LocateCorner(RayDirection.Left, RayDirection.Down);
            endCorner = LocateCorner(RayDirection.Right, RayDirection.Down);
        }

        // adjusting the start points by the object's skin width
        if (rayDirection == RayDirection.Left || rayDirection == RayDirection.Right)
        {
            // change y-coordinates
            startCorner.y -= skinWidth * transform.lossyScale.y;
            endCorner.y += skinWidth * transform.lossyScale.y;
        }
        else
        {
            // change x-coordinates
            startCorner.x += skinWidth * transform.lossyScale.x;
            endCorner.x -= skinWidth * transform.lossyScale.x;
        }
        
        // step 3 : interpolating so that we know the remaining ray origins
        // step 4 : calling raycast on all the ray origins

        float smallestDistance = Mathf.Infinity;

        for (int i = 0; i < accuracyLevel; i++)
        {
            float tValue = (float)i / (float)(accuracyLevel-1f);

            // calculate the ray origin based on an interpolation
            Vector3 rayOrigin = Vector3.Lerp(startCorner, endCorner, tValue);

            // actually cast a ray
            Vector2 rayDirVector = Vector2.zero;
            if (rayDirection == RayDirection.Left) rayDirVector = Vector2.left;
            if (rayDirection == RayDirection.Right) rayDirVector = Vector2.right;
            if (rayDirection == RayDirection.Up) rayDirVector = Vector2.up;
            if (rayDirection == RayDirection.Down) rayDirVector = Vector2.down;
            RaycastHit2D currentResult = Physics2D.Raycast(rayOrigin, rayDirVector, rayDistance);

            Debug.DrawRay(rayOrigin, rayDirVector, Color.yellow);

            if (currentResult.collider == null) continue;

            if (currentResult.distance < smallestDistance)
                result = currentResult;
        }

        // in the end, we should always return something no matter what happens
        return result;
    }

    public Vector3 LocateCorner(RayDirection x, RayDirection y)
    {
        // step 1 : get the object's transform position
        Vector3 result = transform.position;

        // step 2 : get the object's box collider center position (transform.localScale counts)
        Vector3 scaledOffset = new Vector3(
            selfCollider.offset.x * transform.lossyScale.x,
            selfCollider.offset.y * transform.lossyScale.y,
            0
        );
        result += scaledOffset; 

        // step 3 : applying box collider offset (transform.localScale counts)
        if (x == RayDirection.Right)
            result.x += (selfCollider.size.x * 0.5f + skinWidth) * transform.lossyScale.x;
        else if (x == RayDirection.Left)
            result.x -= (selfCollider.size.x * 0.5f + skinWidth) * transform.lossyScale.x;

        // ternary expression:
        // same as using an if statementn like: if (x == RayDirection.Right) isRight = 1; else isRight = -1;
        //float isRight = (x == RayDirection.Right) ? 1 : -1;
        //result.x += selfCollider.size.x * 0.5f * transform.lossyScale.x * isRight;        
        
        // do the same for the Y component
        if (y == RayDirection.Up)
            result.y += (selfCollider.size.y * 0.5f + skinWidth) * transform.lossyScale.y;
        else if (y == RayDirection.Down)
            result.y -= (selfCollider.size.y * 0.5f + skinWidth) * transform.lossyScale.y;

        return result;
    }
}
