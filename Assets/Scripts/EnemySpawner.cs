using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour // Add interface for wave list and config
{
    [System.Serializable]
    public class Wave
    {
        public string waveName; 
        public GameObject enemyPrefab; 
        public float spawnInterval; 
        public int maxEnemies; 
    }

    public List<Wave> waves; 
    public float startDelay = 1f; 
    public float cooldownDuration = 5f; 

    private int currentWaveIndex = 0;
    private int enemiesSpawned = 0;
    private float nextSpawnTime;
    private bool isSpawning = false;

    void Start() // Wait for the wave to start
    {
        StartCoroutine(InitialDelay());
    }

    IEnumerator InitialDelay()
    {
        yield return new WaitForSeconds(startDelay);
        StartWave();
    }

    void StartWave() // Start the current wave
    {
        if (currentWaveIndex < waves.Count)
        {
            Wave currentWave = waves[currentWaveIndex];
            enemiesSpawned = 0;
            isSpawning = true;
            nextSpawnTime = Time.time + currentWave.spawnInterval;
            Debug.Log("Starting Wave: " + currentWave.waveName);
        }
        else
        {
            Debug.Log("All waves completed!");
            isSpawning = false; 
        }
    }

    void Update() // Track how many enemies spawned on the current wave
    {
        if (isSpawning && currentWaveIndex < waves.Count)
        {
            Wave currentWave = waves[currentWaveIndex];

            if (enemiesSpawned < currentWave.maxEnemies && Time.time >= nextSpawnTime)
            {
                SpawnEnemy(currentWave.enemyPrefab);
                enemiesSpawned++;
                nextSpawnTime = Time.time + currentWave.spawnInterval;

                if (enemiesSpawned >= currentWave.maxEnemies)
                {
                    StartCoroutine(Cooldown()); // If all enemies have spawned 
                }
            }
        }
    }

    IEnumerator Cooldown() // Basic wave cooldown before next wave
    {
        isSpawning = false;
        if (currentWaveIndex < waves.Count - 1) 
        {
            yield return new WaitForSeconds(cooldownDuration); 
            currentWaveIndex++;
            StartWave();
        }
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        Instantiate(enemyPrefab, transform.position, transform.rotation);
    }
}
