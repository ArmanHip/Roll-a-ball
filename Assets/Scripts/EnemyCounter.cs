using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyCounter : MonoBehaviour
{
    public TextMeshProUGUI enemyCountText; // Enemy count text

    void Update()
    {
        UpdateEnemyCountText();
    }

    private void UpdateEnemyCountText()
    {
        if (enemyCountText != null)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // Get count of all objects with the tag "enemy"
            enemyCountText.text = $"Enemies: {enemies.Length}";
        }
    }
}
