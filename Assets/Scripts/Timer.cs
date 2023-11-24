using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText; 
    public GameObject winPanel; 
    private float countdown; 

    void Start()
    {
        // Time limit for each level
        if (SceneManager.GetActiveScene().name == "Trial 2")
        {
            countdown = 240.0f; // 4 mins
        }
        else
        {
            countdown = 300.0f; // 5 mins
        }

        winPanel.SetActive(false);
        Time.timeScale = 1;
        UpdateTimerText();
    }

    void Update()
    {
        countdown -= Time.deltaTime;

        if (countdown < 0)
        {
            countdown = 0;
            winPanel.SetActive(true); 
            Time.timeScale = 0; 
            return; 
        }

        UpdateTimerText();

        // If 10 seconds or less, change the color to red
        if (countdown <= 10)
        {
            timerText.color = Color.red;
        }
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(countdown / 60F);
        int seconds = Mathf.FloorToInt(countdown - minutes * 60);
        string formattedTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        
        timerText.text = formattedTime;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1; 
        SceneManager.LoadScene("MainMenu"); 
    }
}