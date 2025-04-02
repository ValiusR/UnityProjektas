using UnityEngine;

public class XPOrb : MonoBehaviour
{
    [SerializeField] public int xpValue = 10; // Adjustable in Inspector
    [SerializeField] public AudioClip collectSound; // Optional sound effect
    [SerializeField] public GameObject collectEffect; // Optional particle effect

    public void OnTriggerEnter2D(Collider2D other)
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
            DestroyImmediate(gameObject);
        }
    }
}