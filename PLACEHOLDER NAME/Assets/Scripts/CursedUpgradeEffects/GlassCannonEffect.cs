using UnityEngine;

public class GlassCannonEffect : MonoBehaviour
{
    [Range(0f, 1f)]
    public float healthReductionPercentage = 0.5f;
    [Range(0f, 2f)]
    public float speedIncreaseMultiplier = 1.2f;

    public void ApplyGlassCannon(GameObject player)
    {
        PlayerHealthController healthController = player.GetComponent<PlayerHealthController>();
        PlayerMovementController movementController = player.GetComponent<PlayerMovementController>();

        if (healthController != null)
        {
            int newMaxHP = Mathf.FloorToInt(healthController.maxHP * (1f - healthReductionPercentage));
            healthController.currHP = Mathf.Min(healthController.currHP, newMaxHP); // Reduce current HP if it exceeds the new max
            healthController.maxHP = newMaxHP;
            Debug.Log($"Glass Cannon: Max HP halved (new max: {healthController.maxHP}, current: {healthController.currHP}).");
        }
        else
        {
            Debug.LogError("GlassCannon: PlayerHealthController component not found on the player.");
        }

        if (movementController != null)
        {
            movementController.maxSpeed *= speedIncreaseMultiplier;
            Debug.Log($"Glass Cannon: Max movement speed increased by {(speedIncreaseMultiplier - 1f) * 100}% (new max speed: {movementController.maxSpeed}).");
        }
        else
        {
            Debug.LogError("GlassCannon: PlayerMovementController component not found on the player.");
        }

        // You might want to destroy this effect component after it's applied
        Destroy(this);
    }
}