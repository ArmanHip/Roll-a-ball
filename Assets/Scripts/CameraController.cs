using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float rotationSpeed = 5.0f;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        Quaternion camTurnAngle = Quaternion.Euler(0, mouseX, 0);
        offset = camTurnAngle * offset;

        transform.position = player.transform.position + offset;
        transform.LookAt(player.transform.position);
    }
}
