using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class EnemyHealthBarManager : MonoBehaviour
{
    [SerializeField] EnemyHealthController enemyHealthController; // Public field to assign the Player script
    [SerializeField] Image healthBarFill;


    void Update()
    {
        UpdateHealthBar(enemyHealthController.currHP, enemyHealthController.maxHP);
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = currentHealth / (float)maxHealth;

        }
    }
}
