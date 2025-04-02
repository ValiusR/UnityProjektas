using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class MapEnemyIntegrationTests
{
    [Test]
    public void Enemies_SpawnWithinLoadedChunks()
    {
        // inicializacija
        var mapController = new GameObject().AddComponent<MapController>();
        mapController.chunkSize = 22f;
        mapController.loadRadius = 1;
        mapController.terrainChunks = new List<GameObject> { new GameObject("TestChunk") };

        // sukuriamos priesu grupes
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

        // papildomi chunks su priesais
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
    [Test]
    public void Enemies_NotSpawned_OutsideLoadRadius()
    {
        // paruošiame pagrindinius komponentus
        var mapController = new GameObject().AddComponent<MapController>();
        mapController.chunkSize = 22f;
        mapController.loadRadius = 1; // tik 1 chunk spinduliu

        // sukuriamas tolimas chunkas (už loadRadius ribų)
        var distantChunkPos = new Vector2Int(2, 2);
        var distantChunk = new GameObject("DistantChunk");
        mapController.terrainChunks = new List<GameObject> { distantChunk };

        // paruošiame priešų spawnerį
        var spawner = new GameObject().AddComponent<EnemySpawner>();
        spawner.waves = new List<EnemySpawner.Wave> {
        new EnemySpawner.Wave {
            enemyGroups = new List<EnemySpawner.EnemyGroup> {
                new EnemySpawner.EnemyGroup {
                    enemyCount = 1,
                    enemyPrefab = new GameObject("EnemyPrefab")
                }
            }
        }
    };

        // žaidėjas centre
        var player = new GameObject("Player");
        player.tag = "Player";
        player.transform.position = Vector3.zero;
        mapController.player = player;
        spawner.player = player.transform;

        // bandome spawninti priešus
        spawner.SpawnEnemies();

        // tikriname ar nepasirodė priešai tolimuose chunks
        bool foundDistantEnemy = false;
        foreach (var chunk in mapController.spawnedChunks)
        {
            if (Vector2Int.Distance(chunk.Key, distantChunkPos) > mapController.loadRadius)
            {
                foreach (Transform child in chunk.Value.transform)
                {
                    if (child.name.Contains("EnemyPrefab"))
                    {
                        foundDistantEnemy = true;
                        break;
                    }
                }
            }
        }

        Assert.IsFalse(foundDistantEnemy,
            "neturėtų būti priešų už loadRadius ribų");
    }
    
}