using UnityEngine;

public class XPOrb : MonoBehaviour
{
    [SerializeField] private int xpValue = 10; // Adjustable in Inspector
    [SerializeField] private GameObject collectEffect; // Optional particle effect

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if colliding with player
        if (other.CompareTag("Player"))
        {
            // Add XP to player
            
            LevelUpSystem.GainXP(xpValue);

            // Play sound
            audioManager.PlaySFX(audioManager.XPOrb);

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