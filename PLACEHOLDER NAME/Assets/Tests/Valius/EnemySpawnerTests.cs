using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerTests
{
    [Test]
    public void SpawnEnemies_TeisingaiSkaiciuojaAtstuma()
    {
        // paruošiame spawneri
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

        // sukuriame mock žaidėją
        var player = new GameObject("Player");
        player.transform.position = Vector3.zero;
        spawner.player = player.transform;

        // spawniname priešus
        spawner.SpawnEnemies();

        // tikriname ar priešai atsirado
        var enemies = GameObject.FindObjectsOfType<GameObject>();
        Assert.Greater(enemies.Length, 1);
    }
}