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
    public Transform player;
    [Header("Spawner Attributes")]
    float spawnTimer;
    public void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        player = playerObject.transform;
        CalculateWaveQuota();
       // SpawnEnemies();
    }

    // Update is called once per frame
    public void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= waves[currentWaveCount].spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemies();
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
    public void SpawnEnemies()
    {
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota)
        {
            foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                if (enemyGroup.spawnCount < enemyGroup.enemyCount)
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
