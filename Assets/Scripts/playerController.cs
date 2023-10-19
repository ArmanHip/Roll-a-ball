using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float baseSpeed = 5.0f;
    public float sizeSpeedFactor = 5.0f; 
    public float maxSpeedCap = 10.0f;
    public float shrinkFactor = 0.1f;
    public float growFactor = 0.1f;
    public float maxScale = 5f;
    public float minScale = 0.1f;

    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    private int count;
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    private float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;

        SetCountText();
        winTextObject.SetActive(false);
        speed = baseSpeed;
    }

    void Update()
    {
        HandlePlayerScaling();
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 2)
        {
            winTextObject.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0.0f;
        cameraForward.Normalize();

        Vector3 movement = (cameraForward * movementY) + (Camera.main.transform.right * movementX);
        movement = Vector3.ClampMagnitude(movement, 1);

        rb.AddForce(movement * speed);

        if (rb.velocity.magnitude > maxSpeedCap)
        {
            rb.velocity = rb.velocity.normalized * maxSpeedCap;
        }
    }

    void HandlePlayerScaling()
    {
        float currentScale = transform.localScale.x;
        if (Input.GetKey(KeyCode.R) && currentScale < maxScale)
        {
            float newScale = Mathf.Min(currentScale + growFactor * Time.deltaTime, maxScale);
            transform.localScale = new Vector3(newScale, newScale, newScale);
        }
        else if (Input.GetKey(KeyCode.E) && currentScale > minScale)
        {
            float newScale = Mathf.Max(currentScale - shrinkFactor * Time.deltaTime, minScale);
            transform.localScale = new Vector3(newScale, newScale, newScale);
        }

        speed = baseSpeed - (sizeSpeedFactor * (transform.localScale.x - 1));
        speed = Mathf.Clamp(speed, 0.1f, maxSpeedCap);

        rb.mass = transform.localScale.x;  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DoorKey"))
        {
            other.gameObject.SetActive(false);
        }
    }
}
