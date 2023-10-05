using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownMovement : MonoBehaviour
{
    public float amplitude = 1.0f; 
    public float speed = 1.0f; 

    private Vector3 startingPosition;

    void Start()
    {
        startingPosition = transform.position;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * speed) * amplitude;
        Vector3 newPosition = startingPosition + new Vector3(0, yOffset, 0);

        transform.position = newPosition;
    }
}