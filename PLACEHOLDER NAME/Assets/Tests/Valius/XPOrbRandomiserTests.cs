using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class XPOrbRandomiserTests
{
    [Test]
    public void SpawnOrbs_SpawninaOrbus()
    {
        // paruošiame randomizerį
        var randomizer = new GameObject().AddComponent<XPOrbRandomiser>();
        randomizer.spawnPoints = new List<XPOrbRandomiser.OrbSpawnData> {
            new XPOrbRandomiser.OrbSpawnData {
                spawnPoint = new GameObject("SpawnPoint")
            }
        };
        randomizer.orbPrefabs = new List<GameObject> { new GameObject("XPOrb") };
        randomizer.spawnChance = 1f;

        // spawniname orbus
        randomizer.SpawnOrbs();

        // tikriname ar orbas atsirado
        Assert.IsTrue(randomizer.spawnPoints[0].hasSpawned);
    }
}