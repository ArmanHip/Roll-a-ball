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
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public float shrinkFactor = 0.9f;
    public float shrinkDuration = 1.0f;
    public float growFactor = 5.0f; 
    public float growDuration = 1.0f; 

    private int count;
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    private Coroutine shrinkCoroutine;

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

    void FixedUpdate()
    {
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
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;

            if (shrinkCoroutine != null)
                StopCoroutine(shrinkCoroutine);

            shrinkCoroutine = StartCoroutine(ShrinkPlayerCoroutine());

            SetCountText();
        }

        if (other.gameObject.CompareTag("Grow"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            StartCoroutine(GrowPlayerCoroutine());
            SetCountText();
        }

        if (other.gameObject.CompareTag("DoorKey"))
        {
            other.gameObject.SetActive(false);
        }
    }

    // Player Scale stuff from youtube

    private IEnumerator ShrinkPlayerCoroutine()
    {
        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = initialScale * shrinkFactor;
        float elapsedTime = 0f;

        while (elapsedTime < shrinkDuration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / shrinkDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }

    private IEnumerator GrowPlayerCoroutine()
    {
        float initialScale = transform.localScale.magnitude;
        float targetScale = initialScale * growFactor;
        float elapsedTime = 0f;

        while (elapsedTime < growDuration)
        {
            float currentScale = Mathf.Lerp(initialScale, targetScale, elapsedTime / growDuration);
            transform.localScale = Vector3.one * currentScale;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = Vector3.one * targetScale;
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
