using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideToSideMovementX : MonoBehaviour
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
        
        float xOffset = Mathf.Sin(Time.time * speed) * amplitude;
        Vector3 newPosition = startingPosition + new Vector3(xOffset, 0, 0);

        
        transform.position = newPosition;
    }
}

