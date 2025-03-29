using UnityEngine;

public class XPOrb : MonoBehaviour
{
    [SerializeField] private int xpValue = 10; // Adjustable in Inspector
    [SerializeField] private AudioClip collectSound; // Optional sound effect
    [SerializeField] private GameObject collectEffect; // Optional particle effect

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if colliding with player
        if (other.CompareTag("Player"))
        {
            // Add XP to player
            LevelUpSystem.GainXP(xpValue);

            // Play sound if assigned
            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            }

            // Spawn effect if assigned
            if (collectEffect != null)
            {
                Instantiate(collectEffect, transform.position, Quaternion.identity);
            }

            // Destroy the orb
            Destroy(gameObject);
        }
    }
}