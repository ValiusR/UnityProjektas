using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MapControllerTests
{
    [UnityTest]
    public IEnumerator GetChunkPosition_CalculatesCorrectPosition()
    {
        // paruošia žemėlapio kontrolerį su testiniais parametrais
        var mapController = new GameObject().AddComponent<MapController>();
        mapController.chunkSize = 22f;
        Vector3 testPosition = new Vector3(45, 30, 0);

        // apskaičiuoja chunk poziciją
        var result = mapController.GetChunkPosition(testPosition);

        // tikrina ar teisingai apskaiciuota pozicija
        Assert.AreEqual(new Vector2Int(2, 1), result);
        yield return null;
    }

    [UnityTest]
    public IEnumerator SpawnChunk_AddsToSpawnedChunks()
    {
        // paruošia žemėlapio kontrolerį su testiniu chunk
        var mapController = new GameObject().AddComponent<MapController>();
        mapController.terrainChunks = new List<GameObject> { new GameObject("TestChunk") };
        var testPos = new Vector2Int(1, 1);

        // spawnina chunk
        mapController.SpawnChunk(testPos);
        yield return null;

        // tikrina ar chunk pridėtas į sąrasą
        Assert.IsTrue(mapController.spawnedChunks.ContainsKey(testPos));
    }

    [UnityTest]
    public IEnumerator UpdateChunks_LoadsCorrectRadius()
    {
        // paruošia žemėlapio sistemą su žaidėju
        var mapController = new GameObject().AddComponent<MapController>();
        mapController.chunkSize = 22f;
        mapController.loadRadius = 1;
        mapController.terrainChunks = new List<GameObject> { new GameObject("TestChunk") };

        var player = new GameObject("Player");
        player.transform.position = Vector3.zero;
        mapController.player = player;

        // atnaujina chunks
        mapController.UpdateChunks();
        yield return null;

        // tikrina ar teisingas kiekis chunk užkrautas
        Assert.AreEqual(9, mapController.spawnedChunks.Count); // (2*radius+1)^2
    }
}
