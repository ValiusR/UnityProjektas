using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    // Start is
    // called before the first frame update
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups;
        public int waveQuota;
        public int spawnInterval;
        public int spawnCount;
        public float maxSpawnDistance = 30f;
        public float minSpawnDistance = 20f;
    }
    public List<Wave> waves;

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount;
        public int spawnCount;
        public GameObject enemyPrefab;
    }

    public int currentWaveCount;
    Transform player;
    [Header("Spawner Attributes")]
    float spawnTimer;
    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        player = playerObject.transform;
        CalculateWaveQuota();
       // SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= waves[currentWaveCount].spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemies();
        }

        // Basic wave advancement: Move to the next wave when all enemies are spawned
        if (currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount >= waves[currentWaveCount].waveQuota)
        {
            currentWaveCount++;
            if (currentWaveCount < waves.Count)
            {
                Debug.Log("Wave " + (currentWaveCount + 1) + " starting!");
                waves[currentWaveCount].spawnCount = 0; // Reset spawn count for the new wave
                foreach (var group in waves[currentWaveCount].enemyGroups)
                {
                    group.spawnCount = 0; // Reset spawn count for enemy groups in the new wave
                }
                CalculateWaveQuota(); // Recalculate quota for the new wave
                spawnTimer = 0f; // Reset spawn timer
            }
            else
            {
                Debug.Log("All waves completed!");
                // You might want to stop spawning or handle the end of the spawning sequence here
                enabled = false; // Example: Disable the spawner
            }
        }
    }
    public void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
        {
            currentWaveQuota += enemyGroup.enemyCount;
        }
        waves[currentWaveCount].waveQuota = currentWaveQuota; 
    }
    void SpawnEnemies()
    {
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota)
        {
            foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                if (enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    bool shouldSpawn = !enemyGroup.enemyName.Contains("rare") || Random.value < 0.1f;

                    if (shouldSpawn)
                    {
                        float xDirection = Random.value > 0.5f ? 1f : -1f;
                        float yDirection = Random.value > 0.5f ? 1f : -1f;

                        Vector2 spawnPosition = new Vector2(
                            player.position.x + Random.Range(xDirection * waves[currentWaveCount].minSpawnDistance,
                                                          xDirection * waves[currentWaveCount].maxSpawnDistance),
                            player.position.y + Random.Range(yDirection * waves[currentWaveCount].minSpawnDistance,
                                                          yDirection * waves[currentWaveCount].maxSpawnDistance)
                        );
                        Instantiate(enemyGroup.enemyPrefab, spawnPosition, Quaternion.identity);

                        enemyGroup.spawnCount++;
                        waves[currentWaveCount].spawnCount++;
                    }
                }
            }
        }
    }
}
