using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class HealthOrb : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int healthRestoreAmount = 10;
    [SerializeField] private GameObject collectEffectPrefab;
    [SerializeField] private AudioClip collectSound;

  //  [Header("References")]
  //  [SerializeField] private PlayerHealthController playerHealthController;

    private void OnTriggerEnter2D(Collider2D other)
    {

        // Check if colliding with player
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerHealthController>(out var healthController))
            {
                healthController.Heal(healthRestoreAmount); // Negative damage = heal
            }
            

            // Destroy the orb
            Destroy(gameObject);
        }
    }
}