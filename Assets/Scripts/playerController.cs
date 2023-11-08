using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public float maxspeed;
    public float jumpForce = 5.0f; 
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    private int count;
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    private bool isGrounded; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;

        SetCountText();
        winTextObject.SetActive(false);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText()
    {
        int totalCount = SceneManager.GetActiveScene().name == "Trial 2" ? 12 : 4; // Get total amount for the current level

        countText.text = "Count: " + count.ToString() + " / " + totalCount.ToString();
        if (count >= totalCount)
        {
            winTextObject.SetActive(true);
            StartCoroutine(LoadNextLevel());
        }
    }

    void Update() // for jumping only
    {
        if (!isGrounded) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; 
        }
    }

    void FixedUpdate()
    {
        isGrounded = false;

        // Check if on the ground
        if (Physics.Raycast(transform.position, -Vector3.up, out RaycastHit hit, 0.5f))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                isGrounded = true;
            }
        }

        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0.0f;
        cameraForward.Normalize();
        Vector3 movement = (cameraForward * movementY) + (Camera.main.transform.right * movementX);
        movement = Vector3.ClampMagnitude(movement, maxspeed);
        rb.AddForce(movement * speed);

        if (rb.velocity.magnitude > maxspeed)
        {
            rb.velocity = rb.velocity.normalized * maxspeed;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp") || other.gameObject.CompareTag("Grow"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }

        if (other.gameObject.CompareTag("DoorKey"))
        {
            other.gameObject.SetActive(false);
        }
    }

    private IEnumerator LoadNextLevel()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(3); // Takes 3 seconds to load next level
        Time.timeScale = 1;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings) // Check for the next level
        {
            SceneManager.LoadScene(nextSceneIndex); // Load it
        }
        else
        {
            SceneManager.LoadScene("MainMenu"); // No other levels to load
            Debug.Log("The player has finished the demo");
        }
    }
}
