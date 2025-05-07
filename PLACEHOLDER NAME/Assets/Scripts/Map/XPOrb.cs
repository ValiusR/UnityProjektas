using UnityEngine;

public class XPOrb : MonoBehaviour
{
    [SerializeField] private int xpValue = 10; // Adjustable in Inspector
    [SerializeField] private GameObject collectEffect; // Optional particle effect

    private AudioManager audioManager;
    private LevelUpSystem levelUpSystem;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        // Find the first LevelUpSystem component in the scene
        levelUpSystem = FindObjectOfType<LevelUpSystem>();

        // Optional: Add error handling if LevelUpSystem is not found
        if (levelUpSystem == null)
        {
            Debug.LogError("No LevelUpSystem component found in the scene!");
            enabled = false; // Disable this script to prevent further errors
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if colliding with player
        if (other.CompareTag("Player"))
        {
            // Add XP to player using the found LevelUpSystem instance
            if (levelUpSystem != null)
            {
                levelUpSystem.GainXP(xpValue);
            }

            // Play sound
            if (audioManager != null)
            {
                audioManager.PlaySFX(audioManager.XPOrb);
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