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

    public static bool WinPanelActive = false;

    void Start()
    {
        winPanel.SetActive(false);

        if (SceneManager.GetActiveScene().name == "Level_2")
        {
            countdown = 240.0f; // 4 mins
        }
        else
        {
            countdown = 300.0f; // 5 mins
        }

        Time.timeScale = 1;
        UpdateTimerText();
    }

    void Update()
    {
        if (!WinPanelActive) 
        {
            countdown -= Time.deltaTime;

            if (countdown <= 0)
            {
                countdown = 0;
                WinPanelActive = true;
                winPanel.SetActive(true);
                Time.timeScale = 0;
            }

            UpdateTimerText();

            if (countdown <= 10) // Change color of text to red
            {
                timerText.color = Color.red;
            }
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
        WinPanelActive = false;
        Time.timeScale = 1; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        WinPanelActive = false;
        Time.timeScale = 1; 
        SceneManager.LoadScene("MainMenu"); 
    }
}