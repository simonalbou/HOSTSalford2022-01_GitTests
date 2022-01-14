using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RayDirection { Left, Right, Up, Down }

public class Raycaster2D : MonoBehaviour
{
    public float rayDistance;
    public BoxCollider2D selfCollider; // we'll need this reference to calculate ray origins
    
    [Range(2, 8)] // makes the thing into a slider in Unity's inspector
    public int accuracyLevel;

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
            RaycastHit2D result = Physics2D.Raycast(rayTransform.position, rayTransform.up, rayDistance);

            // checking if result.collider exists ("is not equal to null"), which would mean we hit something
            if (result.collider != null)
            {
                Debug.Log("we hit "+result.collider.name);
            }
        }       
    }

    public RaycastHit2D ThrowRays(RayDirection rayDirection)
    {
        // This variable will hold the result. By default, it's all empty.
        RaycastHit2D result = new RaycastHit2D();

        // This is what we're gonna do in the next session:
        // step 1 : knowing which corners we actually need
        // step 2 : calculate the position of said corners
        // step 3 : interpolating so that we know the remaining ray origins
        // step 4 : calling raycast on all the ray origins

        // in the end, we should always return something no matter what happens
        return result;
    }
}
