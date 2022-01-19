using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    public Transform playerTransform;
    Vector3 initialPlayerPosition; 

    public ParallaxElement[] parallaxElements;

    void Start()
    {
        initialPlayerPosition = playerTransform.position;
    }

    void Update()
    {
        for (int i = 0; i < parallaxElements.Length; i++)
        {
            parallaxElements[i].SetDistance(playerTransform.position.x - initialPlayerPosition.x);
        }
    }
}
