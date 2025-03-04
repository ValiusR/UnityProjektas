using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthBarManager : MonoBehaviour
{
    [SerializeField] PlayerController playerController; // Public field to assign the Player script
    [Header("Image")]
    [SerializeField] Image healthBarFill;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update() 
    {
        if (playerController != null)
        {
            UpdateHealthBar(playerController.currHP, playerController.maxHP);
        }
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        Debug.Log("Current HEALTH: " + currentHealth);
        Debug.Log("Max health: " + maxHealth);
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = currentHealth / (float)maxHealth;
        }
    }
}
