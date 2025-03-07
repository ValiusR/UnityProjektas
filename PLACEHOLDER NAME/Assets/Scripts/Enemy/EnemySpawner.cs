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
    }
    void CalculateWaveQuota()
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
                    Vector2 spawnPosition = new Vector2(player.transform.position.x
                        + Random.Range(-30f, 30f), player.transform.position.y + Random.Range(-30f, 30f));
                    Instantiate(enemyGroup.enemyPrefab, spawnPosition, Quaternion.identity);

                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                }
            }
        }
    }
}
