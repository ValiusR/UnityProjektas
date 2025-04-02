using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class MapPropIntegrationTests
{
    [Test]
    public void ChunkLoading_SpawnsProps_InActiveChunks()
    {
        // paruošiame pagrindines dalis
        var mapController = new GameObject().AddComponent<MapController>();
        mapController.chunkSize = 22f;
        mapController.loadRadius = 1;

        // sukuriame testinį chunk su spawn points
        var testChunk = new GameObject("TestChunk");
        var propContainer = new GameObject("PropLocations");
        propContainer.transform.SetParent(testChunk.transform);

        // pridedame spawn points
        var spawnPoint1 = new GameObject("SpawnPoint1");
        spawnPoint1.transform.SetParent(propContainer.transform);
        spawnPoint1.transform.localPosition = Vector3.zero;

        mapController.terrainChunks = new List<GameObject> { testChunk };

        // paruošiame propų spawninimo sistemą
        var propRandomizer = new GameObject().AddComponent<PropRandomizer>();

        // inicializuojame privalomus laukus
        propRandomizer.propSpawnPoints = new List<GameObject> { spawnPoint1 }; // privaloma!
        propRandomizer.propPrefabs = new List<GameObject> { new GameObject("TestProp") };

        // sukuriame žaidėją
        var player = new GameObject("Player");
        player.transform.position = Vector3.zero;
        mapController.player = player;

      
        Vector2Int chunkPos = mapController.GetChunkPosition(player.transform.position);
        mapController.SpawnChunk(chunkPos);

        // sukuriame props
        propRandomizer.SpawnProps(); // dabar neturėtų mesti NullReferenceException

        // tikriname rezultatus
        Assert.IsTrue(mapController.chunkProps.ContainsKey(chunkPos),
            "chunkas turėtų būti užregistruotas");

        // papildomas patikrinimas - ar prop atsirado spawn pointe
        bool radoPropą = spawnPoint1.transform.childCount > 0;
        Assert.IsTrue(radoPropą,
            "spawn point'e turėtų būti spawnintas propas");
    }
    [Test]
    public void Props_NotSpawned_InUnloadedChunks()
    {
        // paruošiame pagrindinius komponentus
        var mapController = new GameObject().AddComponent<MapController>();
        mapController.chunkSize = 22f;
        mapController.loadRadius = 1;

        // sukuriame chunk toli nuo žaidėjo
        var distantChunk = new GameObject("DistantChunk");
        var propContainer = new GameObject("PropLocations");
        propContainer.transform.SetParent(distantChunk.transform);

        var spawnPoint = new GameObject("SpawnPoint");
        spawnPoint.transform.SetParent(propContainer.transform);
        spawnPoint.transform.localPosition = new Vector3(50f, 50f, 0f); // toli nuo žaidėjo

        mapController.terrainChunks = new List<GameObject> { distantChunk };

        // paruošiame props sistema
        var propRandomizer = new GameObject().AddComponent<PropRandomizer>();
        propRandomizer.propSpawnPoints = new List<GameObject> { spawnPoint };
        propRandomizer.propPrefabs = new List<GameObject> { new GameObject("TestProp") };

        // žaidėjas lieka pradinėje pozicijoje (0,0,0)
        var player = new GameObject("Player");
        player.transform.position = Vector3.zero;
        mapController.player = player;

        // vykdome spawninimą
        propRandomizer.SpawnProps();

        // tikriname ar propas nespawnino tolimame chunk
        Assert.AreEqual(1, spawnPoint.transform.childCount,
            "propas neturėtų spawninti neaktyviame chunke");
    }
    [Test]
    public void MultipleProps_SpawnAtDifferentPoints()
    {
        // paruošiame pagrindinius komponentus
        var mapController = new GameObject().AddComponent<MapController>();
        mapController.chunkSize = 22f;
        mapController.loadRadius = 1;

        // sukuriame chunk'ą su keliais spawn point'ais
        var testChunk = new GameObject("TestChunk");
        var propContainer = new GameObject("PropLocations");
        propContainer.transform.SetParent(testChunk.transform);

        var spawnPoint1 = new GameObject("SpawnPoint1");
        var spawnPoint2 = new GameObject("SpawnPoint2");
        spawnPoint1.transform.SetParent(propContainer.transform);
        spawnPoint2.transform.SetParent(propContainer.transform);

        mapController.terrainChunks = new List<GameObject> { testChunk };

        // paruošiame propų sistemą su dviem skirtingais prefabais
        var propRandomizer = new GameObject().AddComponent<PropRandomizer>();
        propRandomizer.propSpawnPoints = new List<GameObject> { spawnPoint1, spawnPoint2 };
        propRandomizer.propPrefabs = new List<GameObject> {
        new GameObject("PropType1"),
        new GameObject("PropType2")
        };

        // žaidėjas pradinėje pozicijoje
        var player = new GameObject("Player");
        player.transform.position = Vector3.zero;
        mapController.player = player;

        // spawniname chunk ir props
        Vector2Int chunkPos = mapController.GetChunkPosition(player.transform.position);
        mapController.SpawnChunk(chunkPos);
        propRandomizer.SpawnProps();

        // tikriname rezultatus
        Assert.AreEqual(2, mapController.chunkProps[chunkPos].Count,
            "turėtų būti spawninti 2 propai");

        Assert.IsTrue(spawnPoint1.transform.childCount > 0 && spawnPoint2.transform.childCount > 0,
            "abu spawn point'ai turėtų turėti vaikus");
    }
}