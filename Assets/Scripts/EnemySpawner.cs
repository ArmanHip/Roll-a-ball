using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable] // Wave settings ect
    public class Wave
    {
        public string waveName;
        public float spawnInterval;
        public int maxEnemies;

        public GameObject enemyPrefab;
    }

    public List<Wave> waves;
    public float startDelay = 1f;
    public float cooldownDuration = 5f;

    private int currentWaveIndex = 0;

    public int enemiesSpawned = 0; // has to be public for wave info stuff
    private float nextSpawnTime;
    private bool isSpawning = false;

    public int CurrentWaveIndex { get { return currentWaveIndex; } } // Used for wave info stuff
    public float NextSpawnTime { get { return nextSpawnTime; } }
    public bool IsSpawning { get { return isSpawning; } }
    public float CooldownDuration { get { return cooldownDuration; } }
    public float WaveStartTime { get; private set; }

    public List<Wave> Waves { get { return waves; } }

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
            WaveStartTime = Time.time;
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

    public float CalculateFullWaveTime() // Simple way to get the full wave time, used for displaying the wave time
    {
        if (currentWaveIndex < waves.Count)
        {
            Wave currentWave = waves[currentWaveIndex];
            return currentWave.maxEnemies * currentWave.spawnInterval;
        }
        return 0f;
    }
}
