using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputTest : MonoBehaviour
{
    public MeshRenderer selfRenderer;
    public Camera cam;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit result;
            if (Physics.Raycast(ray.origin, ray.direction, out result, 200))
            {
                if (result.collider.gameObject == gameObject)
                    selfRenderer.material.color = Color.blue;          
            }
        }
    }

    void OnMouseDown()
    {
        //selfRenderer.material.color = Color.red;
    }
}
