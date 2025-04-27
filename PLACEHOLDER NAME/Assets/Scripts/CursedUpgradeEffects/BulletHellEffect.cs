using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class BulletHellEffect : MonoBehaviour
{
    public float newSpawnInterval = 2f; // Example: Decrease spawn interval significantly
    private EnemySpawner enemySpawner;
    private float originalSpawnInterval;
    private bool effectApplied = false;

    public void ApplyBulletHell(GameObject player)
    {
        if (effectApplied) return;

        enemySpawner = FindObjectOfType<EnemySpawner>();
        if (enemySpawner != null)
        {
            // Store the original spawn interval of the *first* wave (you might need a more sophisticated way if intervals change per wave)
            if (enemySpawner.waves.Count > 0)
            {
                originalSpawnInterval = enemySpawner.waves[enemySpawner.currentWaveCount].spawnInterval;
                // Decrease the spawn interval for the current wave (you might want to do this for all waves too)
                enemySpawner.waves[enemySpawner.currentWaveCount].spawnInterval = Mathf.Min((int)newSpawnInterval, enemySpawner.waves[enemySpawner.currentWaveCount].spawnInterval);
                Debug.Log("Bullet Hell Effect: Spawn interval decreased for the current wave to " + enemySpawner.waves[enemySpawner.currentWaveCount].spawnInterval);
            }

            // Remove Bat and Golem type enemies from ALL waves
            foreach (var wave in enemySpawner.waves)
            {
                wave.enemyGroups.RemoveAll(group =>
                    group.enemyName == "Bat" ||
                    group.enemyName == "Golem" ||
                    group.enemyName == "Bat (rare)" ||
                    group.enemyName == "Golem (rare)");
                wave.waveQuota = 0; // Reset wave quota before recalculating
                foreach (var group in wave.enemyGroups)
                {
                    wave.waveQuota += group.enemyCount;
                }
            }
            Debug.Log("Bullet Hell Effect: Removed Bat and Golem type enemies from ALL waves.");

            effectApplied = true;
        }
        else
        {
            Debug.LogError("Bullet Hell Effect: EnemySpawner not found in the scene.");
        }

        // You might want to destroy this effect component after it's applied
        Destroy(this);
    }

    // Optional: Implement a method to revert the changes if the curse is temporary
    public void RevertBulletHell()
    {
        // Reverting this kind of permanent change across all waves would be more complex.
        // You would likely need to store a copy of the original waves list before applying the curse.
        if (enemySpawner != null && effectApplied)
        {
            // This is a placeholder for more complex reversion logic
            Debug.LogWarning("Bullet Hell Effect: Reverting enemy removal across all waves is not implemented in this example.");
            // To revert, you would need to restore the original enemySpawner.waves list.
            if (enemySpawner.waves.Count > 0)
            {
                enemySpawner.waves[enemySpawner.currentWaveCount].spawnInterval = (int)originalSpawnInterval;
                Debug.Log("Bullet Hell Effect: Spawn interval for the current wave reverted to " + originalSpawnInterval);
            }
            effectApplied = false;
        }
    }
}