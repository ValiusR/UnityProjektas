using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class MapEnemyIntegrationTests
{
    [Test]
    public void Enemies_SpawnWithinLoadedChunks()
    {
        // setup
        var mapController = new GameObject().AddComponent<MapController>();
        mapController.chunkSize = 22f;
        mapController.loadRadius = 1;
        mapController.terrainChunks = new List<GameObject> { new GameObject("TestChunk") };

        var spawner = new GameObject().AddComponent<EnemySpawner>();
        spawner.waves = new List<EnemySpawner.Wave> {
            new EnemySpawner.Wave {
                minSpawnDistance = 5f,
                maxSpawnDistance = 10f,
                enemyGroups = new List<EnemySpawner.EnemyGroup> {
                    new EnemySpawner.EnemyGroup {
                        enemyCount = 1,
                        enemyPrefab = new GameObject("EnemyPrefab")
                    }
                }
            }
        };
        var player = new GameObject("Player");
        player.tag = "Player";
        player.transform.position = Vector3.zero;
        mapController.player = player;
        spawner.player = player.transform;

        mapController.SpawnChunk(mapController.GetChunkPosition(player.transform.position));
        spawner.SpawnEnemies();

        var activeChunk = mapController.GetChunkPosition(player.transform.position);
        bool allValid = true;

        foreach (Transform child in mapController.spawnedChunks[activeChunk].transform)
        {
            if (child.name.Contains("EnemyPrefab"))
            {
                var enemyChunk = mapController.GetChunkPosition(child.position);
                if (Vector2Int.Distance(activeChunk, enemyChunk) > 1)
                {
                    allValid = false;
                    break;
                }
            }
        }

        Assert.IsTrue(allValid);
    }
}