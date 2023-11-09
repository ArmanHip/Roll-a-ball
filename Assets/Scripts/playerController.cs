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
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 9f;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public TextMeshProUGUI dashCooldownText; 
    public GameObject dashCooldownBox; 

    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    private bool isGrounded; 

    private bool isDashing;
    private float dashTimer;
    private float dashCooldownTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;

        SetCountText();
        winTextObject.SetActive(false);

        isDashing = false;
        dashTimer = 0;
        dashCooldownTimer = 0;

        dashCooldownBox.SetActive(true);
        dashCooldownText.gameObject.SetActive(false);
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

    void Update()
    {
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
            dashCooldownText.gameObject.SetActive(true); 
            dashCooldownText.text = dashCooldownTimer.ToString("F1") + "s";
        }
        else
        {
            dashCooldownText.gameObject.SetActive(false); 
        }

        if (!isGrounded) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        // Dash 
        if (Input.GetKeyDown(KeyCode.R) && dashCooldownTimer <= 0 && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }


    private IEnumerator Dash()
    {
        float startTime = Time.time; 
        isDashing = true;

        Vector3 dashDirection = Camera.main.transform.forward;
        dashDirection.y = 0;
        dashDirection.Normalize();

        rb.velocity = dashDirection * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
        dashCooldownTimer = dashCooldown; 
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0.0f;
            cameraForward.Normalize();
            Vector3 movement = (cameraForward * movementY) + (Camera.main.transform.right * movementX);
            movement = Vector3.ClampMagnitude(movement, 1); 

            rb.AddForce(movement * speed);

            if (rb.velocity.magnitude > maxspeed)
            {
                rb.velocity = rb.velocity.normalized * maxspeed;
            }
        }

        isGrounded = false;

        // Check if on the ground
        if (Physics.Raycast(transform.position, -Vector3.up, out RaycastHit hit, 0.5f))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                isGrounded = true;
            }
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
