using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float rotationSpeed = 5.0f;
    public float minDistance = 3.0f;
    public float maxPlayerScale = 2.0f;
    public float cameraLagSpeed = 5.0f; 
    public Camera cam; 

    private Vector3 offset;
    private float baseFOV; 

    void Start()
    {
        CalculateOffset();
        baseFOV = cam.fieldOfView; 
    }

    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        Quaternion camTurnAngle = Quaternion.Euler(0, mouseX, 0);
        offset = camTurnAngle * offset;

        float playerScale = Mathf.Clamp(player.transform.localScale.magnitude, 1.0f, maxPlayerScale);
        float maxDistance = playerScale * 5.0f;

        Vector3 desiredPosition = player.transform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, cameraLagSpeed * Time.deltaTime); 

        float distance = Vector3.Distance(smoothedPosition, transform.position);

        if (distance > maxDistance)
            smoothedPosition = player.transform.position + offset.normalized * maxDistance;
        else if (distance < minDistance)
            smoothedPosition = player.transform.position + offset.normalized * minDistance;

        transform.position = smoothedPosition;

        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        float speed = playerRb.velocity.magnitude;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, baseFOV + speed, Time.deltaTime);

        transform.LookAt(player.transform.position);
    }

    void CalculateOffset()
    {
        float playerScale = Mathf.Max(player.transform.localScale.x, player.transform.localScale.y, player.transform.localScale.z);
        offset = new Vector3(0, playerScale * 2.5f, -playerScale * 5.0f);
    }
}
