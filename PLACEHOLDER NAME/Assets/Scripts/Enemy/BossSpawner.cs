using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private float spawnTime = 60f; // Time in seconds
    [SerializeField] private float spawnDistance = 5f; // Distance from player
    [SerializeField] private Transform player;
    private AudioManager audioManager;

    private float timer;
    private bool bossSpawned = false;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Update()
    {
        if (bossSpawned) return;

        timer += Time.deltaTime;

        if (timer >= spawnTime)
        {
            SpawnBoss();
            bossSpawned = true;
        }
    }

    void SpawnBoss()
    {
        if (bossPrefab != null && player != null)
        {
            
            PlayerBoundary playerBoundary = player.GetComponent<PlayerBoundary>();


            Vector2 randomOffset = Random.insideUnitCircle.normalized * spawnDistance;
            Vector3 spawnPosition = player.position + new Vector3(randomOffset.x, randomOffset.y, 0);

            Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
            playerBoundary.CreateBoundaryOnBossSpawn();

            audioManager.PlayBossMusic();
        }
    }
}
