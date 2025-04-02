using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class MapXPOrbIntegrationTests
{
    [Test]
    public void XPOrbai_Atsiranda_TikAktyviuoseChunkuose()
    {
        // paruošiame pagrindinius komponentus
        var mapController = new GameObject().AddComponent<MapController>();
        mapController.chunkSize = 22f;
        mapController.loadRadius = 1;

        // sukuriame testinį chunk su orbų spawn point
        var testChunk = new GameObject("TestChunk");
        var orbSpawnPoint = new GameObject("OrbSpawnPoint");
        orbSpawnPoint.transform.SetParent(testChunk.transform);
        orbSpawnPoint.transform.localPosition = Vector3.zero;

        mapController.terrainChunks = new List<GameObject> { testChunk };

        // inicializacija randomizer
        var orbRandomizer = new GameObject().AddComponent<XPOrbRandomiser>();

        
        orbRandomizer.spawnPoints = new List<XPOrbRandomiser.OrbSpawnData> {
            new XPOrbRandomiser.OrbSpawnData { spawnPoint = orbSpawnPoint }
        };
        orbRandomizer.orbPrefabs = new List<GameObject> { new GameObject("XPOrbPrefab") };
        orbRandomizer.spawnChance = 1f; // 100% spawn tikimybė testui

        // zaidejas
        var player = new GameObject("Player");
        player.transform.position = Vector3.zero;
        mapController.player = player;

        //  testo vykdymas
        Vector2Int chunkPos = mapController.GetChunkPosition(player.transform.position);
        mapController.SpawnChunk(chunkPos);
        orbRandomizer.SpawnOrbs();

        // tikriname rezultatus
        // patikriname ar orbas atsirado spawn point'e
        bool radoOrbą = orbSpawnPoint.transform.childCount > 0;
        Assert.IsTrue(radoOrbą, "spawn point'e turėtų būti XP orbas");

        // patikriname ar orbas teisingame chunke
        if (radoOrbą)
        {
            var orbas = orbSpawnPoint.transform.GetChild(0).gameObject;
            var orbChunkPos = mapController.GetChunkPosition(orbas.transform.position);
            Assert.AreEqual(chunkPos, orbChunkPos,
                "orbas turėtų būti to paties chunko ribose");
        }

        // papildomai patikriname per chunkProps sąrašą
        Assert.IsTrue(mapController.chunkProps.ContainsKey(chunkPos),
            "chunkas turėtų būti užregistruotas");
      
    }
}