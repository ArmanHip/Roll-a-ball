using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveInfo : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    public TextMeshProUGUI waveInfoText;
    public TextMeshProUGUI timeInfoText;

    void Update() // Update text with wave info
    {
        if (enemySpawner != null && waveInfoText != null && timeInfoText != null)
        {
            UpdateWaveInfoText();
            UpdateTimeInfoText();
        }
    }

    private void UpdateWaveInfoText()
    {
        waveInfoText.text = $"Wave {enemySpawner.CurrentWaveIndex + 1} / {enemySpawner.Waves.Count}"; // Display current wave
    }

    private void UpdateTimeInfoText()
    {
        if (enemySpawner.IsSpawning)
        {
            float fullWaveTime = enemySpawner.CalculateFullWaveTime();
            float timeSinceWaveStart = Time.time - enemySpawner.WaveStartTime; // Get time since wave started
            float timeRemaining = Mathf.Max(fullWaveTime - timeSinceWaveStart, 0); // Get time
            timeInfoText.text = $"{Mathf.CeilToInt(timeRemaining)} Seconds Remaining"; // Display time in whole seconds so it's nice and clean
        }
        else
        {
            timeInfoText.text = "Next wave will start soon!";
        }
    }
}
