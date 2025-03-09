using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class HealthBarManager : MonoBehaviour
{
    [SerializeField] PlayerHealthController playerController; // Public field to assign the Player script
    [Header("Image")]
    [SerializeField] Image healthBarFill;
    [SerializeField] TextMeshProUGUI healthMeterText;

    void Start()
    {
    }

    void Update()
    {
        UpdateHealthBar(playerController.currHP, playerController.maxHP);
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        //Debug.Log("Current HEALTH: " + currentHealth);
        //Debug.Log("Max health: " + maxHealth);
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = currentHealth / (float)maxHealth;

        }
        if (healthMeterText != null)
        {
            healthMeterText.text = currentHealth + "/" + maxHealth;
        }
    }
}
