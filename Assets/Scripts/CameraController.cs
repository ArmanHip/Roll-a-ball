using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float rotationspeed = 5.0f;
    public float mindistance = 3.0f;
    public float maxPlayerScale = 2.0f;

    private Vector3 offset;

    void Start()
    {
        CalculateOffset();
    }

    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationspeed;
        Quaternion camTurnAngle = Quaternion.Euler(0, mouseX, 0);
        offset = camTurnAngle * offset;

        float playerScale = Mathf.Clamp(player.transform.localScale.magnitude, 1.0f, maxPlayerScale);
        float maxdistance = playerScale * 5.0f;

        Vector3 desiredPosition = player.transform.position + offset;

        float distance = Vector3.Distance(desiredPosition, transform.position);

        if (distance > maxdistance)
            desiredPosition = player.transform.position + offset.normalized * maxdistance;
        else if (distance < mindistance)
            desiredPosition = player.transform.position + offset.normalized * mindistance;

        transform.position = desiredPosition;
        transform.LookAt(player.transform.position);
    }

    void CalculateOffset()
    {
        float playerScale = Mathf.Max(player.transform.localScale.x, player.transform.localScale.y, player.transform.localScale.z);
        offset = new Vector3(0, playerScale * 2.5f, -playerScale * 5.0f);
    }
}