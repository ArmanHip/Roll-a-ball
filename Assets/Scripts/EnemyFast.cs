using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFast : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 7f;
    public float rotationSpeed = 5f;
    public float smoothTime = 1F;

    private Rigidbody rb;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0; 

        if (direction.magnitude > 0.1f) 
        {
            direction.Normalize();

            Vector3 targetPosition = transform.position + direction * moveSpeed;
            rb.MovePosition(Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime));

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}
