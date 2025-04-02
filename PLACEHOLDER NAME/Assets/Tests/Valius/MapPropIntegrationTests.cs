using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class MapPropIntegrationTests
{
    [Test]
    public void ChunkLoading_SpawnsProps_InActiveChunks()
    {
        // 1. paruošiame pagrindinius komponentus
        var mapController = new GameObject().AddComponent<MapController>();
        mapController.chunkSize = 22f;
        mapController.loadRadius = 1;

        // sukuriame testinį chunk'ą su spawn point'ais
        var testChunk = new GameObject("TestChunk");
        var propContainer = new GameObject("PropLocations");
        propContainer.transform.SetParent(testChunk.transform);

        // pridedame spawn point'us
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

        // 2. vykdome testą
        Vector2Int chunkPos = mapController.GetChunkPosition(player.transform.position);
        mapController.SpawnChunk(chunkPos);

        // spawniname propus
        propRandomizer.SpawnProps(); // dabar neturėtų mesti NullReferenceException

        // 3. tikriname rezultatus
        Assert.IsTrue(mapController.chunkProps.ContainsKey(chunkPos),
            "chunkas turėtų būti užregistruotas");

        // papildomas patikrinimas - ar propas atsirado spawn point'e
        bool radoPropą = spawnPoint1.transform.childCount > 0;
        Assert.IsTrue(radoPropą,
            "spawn point'e turėtų būti spawnintas propas");
    }
}