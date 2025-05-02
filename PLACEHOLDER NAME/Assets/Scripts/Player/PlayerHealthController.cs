using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] DamageBlink damageBlink;
    [SerializeField] public DeathUiManager deathUIManager; // Assign your Options UI Prefab in the Inspector
    public int maxHP;
    public int currHP;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public void ForceDeath()
    {
        // Immediately set health to 0
        currHP = 0;

        // Disable movement components
        var movement = GetComponent<PlayerMovementController>();
        if (movement != null) movement.enabled = false;

        // Stop any physics
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.velocity = Vector2.zero;

        // Show death UI
        if (deathUIManager != null)
        {
            deathUIManager.ShowDeathUI();
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }

        // Ensure game is paused
        Time.timeScale = 0f;
        Debug.Log("Game should be paused now - Time.timeScale: " + Time.timeScale);
    }
    public void Heal(int healthToAdd)
    {
        if (currHP+healthToAdd >= maxHP)
        {
            currHP = maxHP;
        }
        else
        {
           
            currHP += healthToAdd;
        }
    }
    public void TakeDamage(int damage)
    {
        if(currHP > damage)
        {
            currHP -= damage;
            audioManager.PlaySFX(audioManager.playerHit);
        }
        else
        {
            currHP = 0;
        }

        //Play hurt animation
        damageBlink.PlayBlink();

        //Death
        if (currHP <= 0)
        {
            if (deathUIManager != null)
            {
                deathUIManager.ShowDeathUI();
            }
            else
            {
                SceneManager.LoadScene("MainMenu");
            }
           // SceneManager.LoadScene("MainMenu");
        }
    }
}
