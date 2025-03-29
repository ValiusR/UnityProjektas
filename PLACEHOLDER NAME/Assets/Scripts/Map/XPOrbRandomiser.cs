using System.Collections.Generic;
using UnityEngine;

public class XPOrbRandomiser : MonoBehaviour
{
    [System.Serializable]
    public class OrbSpawnData
    {
        public GameObject spawnPoint;
        public bool hasSpawned = false;
    }

    [SerializeField] private List<OrbSpawnData> spawnPoints = new List<OrbSpawnData>();
    [SerializeField] private List<GameObject> orbPrefabs = new List<GameObject>();
    [Range(0f, 1f)] public float spawnChance = 0.5f;

    private void Start()
    {
        InitializeSpawnPoints();
        SpawnOrbs();
    }

    private void InitializeSpawnPoints()
    {
        // Initialize spawn points if empty
        if (spawnPoints == null)
        {
            spawnPoints = new List<OrbSpawnData>();
        }

        // Initialize orb prefabs if empty
        if (orbPrefabs == null)
        {
            orbPrefabs = new List<GameObject>();
        }
    }

    private void SpawnOrbs()
    {
        if (orbPrefabs.Count == 0)
        {
            Debug.LogWarning("No orb prefabs assigned!", this);
            return;
        }

        foreach (var spawnData in spawnPoints)
        {
            if (spawnData.spawnPoint == null) continue;
            if (spawnData.hasSpawned) continue;

            if (Random.value <= spawnChance)
            {
                SpawnOrbAtPoint(spawnData);
            }
        }
    }

    private void SpawnOrbAtPoint(OrbSpawnData spawnData)
    {
        int randomIndex = Random.Range(0, orbPrefabs.Count);
        GameObject orb = Instantiate(
            orbPrefabs[randomIndex],
            spawnData.spawnPoint.transform.position,
            Quaternion.identity,
            spawnData.spawnPoint.transform
        );

        spawnData.hasSpawned = true;
    }

    // Editor-only validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        InitializeSpawnPoints();
    }
#endif
}