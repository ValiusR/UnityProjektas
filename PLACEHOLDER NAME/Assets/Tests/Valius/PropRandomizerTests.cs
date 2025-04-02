using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PropRandomizerTests
{
    [Test]
    public void SpawnProps_InstantinaPropsus()
    {
        // paruošiame randomizerį
        var randomizer = new GameObject().AddComponent<PropRandomizer>();
        randomizer.propSpawnPoints = new List<GameObject> {
            new GameObject("SpawnPoint1"),
            new GameObject("SpawnPoint2")
        };
        randomizer.propPrefabs = new List<GameObject> { new GameObject("PropPrefab") };

        // spawniname propsus
        randomizer.SpawnProps();

        // tikriname ar propsai atsirado
        foreach (var point in randomizer.propSpawnPoints)
        {
            Assert.Greater(point.transform.childCount, 0);
        }
    }
}