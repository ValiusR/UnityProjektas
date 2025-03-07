using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> terrainChunks;
    public GameObject player;
    public float chunkSize = 22f; // Adjust based on chunk dimensions
    public int loadRadius = 1; // Number of chunks to load around the player

    private Dictionary<Vector2Int, GameObject> spawnedChunks = new Dictionary<Vector2Int, GameObject>();
    private Dictionary<Vector2Int, List<GameObject>> chunkProps = new Dictionary<Vector2Int, List<GameObject>>();

    void Update()
    {
        UpdateChunks();
    }

    void UpdateChunks()
    {
        Vector2Int playerChunkPos = GetChunkPosition(player.transform.position);

        // Load chunks within the specified radius
        for (int x = -loadRadius; x <= loadRadius; x++)
        {
            for (int y = -loadRadius; y <= loadRadius; y++)
            {
                Vector2Int chunkPos = playerChunkPos + new Vector2Int(x, y);
                if (!spawnedChunks.ContainsKey(chunkPos))
                {
                    SpawnChunk(chunkPos);
                }
                else
                {
                    // Re-enable props if they were disabled
                    SetChunkPropsActive(chunkPos, true);
                }
            }
        }

        // Remove chunks that are too far
        List<Vector2Int> chunksToRemove = new List<Vector2Int>();
        foreach (var chunk in spawnedChunks)
        {
            if (Vector2Int.Distance(chunk.Key, playerChunkPos) > loadRadius + 1)
            {
                SetChunkPropsActive(chunk.Key, false); // Disable props before removing chunk
                Destroy(chunk.Value);
                chunksToRemove.Add(chunk.Key);
            }
        }

        foreach (var chunkPos in chunksToRemove)
        {
            spawnedChunks.Remove(chunkPos);
        }
    }

    void SpawnChunk(Vector2Int chunkPos)
    {
        int rand = Random.Range(0, terrainChunks.Count);
        Vector3 worldPos = new Vector3(chunkPos.x * chunkSize, chunkPos.y * chunkSize, 0);
        GameObject newChunk = Instantiate(terrainChunks[rand], worldPos, Quaternion.identity);
        spawnedChunks[chunkPos] = newChunk;

        // Find and store props inside the chunk
        List<GameObject> props = new List<GameObject>();
        Transform propContainer = newChunk.transform.Find("PropLocations"); // Assumes props are in a child object named "PropLocations"
        if (propContainer != null)
        {
            foreach (Transform prop in propContainer)
            {

                props.Add(prop.gameObject);
            }
        }
        chunkProps[chunkPos] = props;
    }

    void SetChunkPropsActive(Vector2Int chunkPos, bool active)
    {
        if (chunkProps.ContainsKey(chunkPos))
        {
            foreach (GameObject prop in chunkProps[chunkPos])
            {
                if (prop != null)
                {
                    // Disable SpriteRenderer if present
                    SpriteRenderer spriteRenderer = prop.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.enabled = active;
                    }

                    // Disable CanvasRenderer if present (for UI elements)
                    CanvasRenderer canvasRenderer = prop.GetComponent<CanvasRenderer>();
                    if (canvasRenderer != null)
                    {
                        canvasRenderer.SetAlpha(active ? 1f : 0f);
                    }

                    // If you have other renderers, add similar code here.

                    //Also disable the game object itself.
                    prop.SetActive(active);
                }
            }
        }
    }

    Vector2Int GetChunkPosition(Vector3 position)
    {
        return new Vector2Int(Mathf.FloorToInt(position.x / chunkSize), Mathf.FloorToInt(position.y / chunkSize));
    }
}
