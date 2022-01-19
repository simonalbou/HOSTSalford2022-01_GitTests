using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxElement : MonoBehaviour
{
    public float speed;
    Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    public void SetDistance(float newDistance)
    {
        transform.position = initialPosition + Vector3.left * newDistance * speed;
    }
}
