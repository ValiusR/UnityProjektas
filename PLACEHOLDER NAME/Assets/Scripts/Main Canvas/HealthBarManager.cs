using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarManager : MonoBehaviour
{
    [SerializeField] public PlayerHealthController playerController;
    [SerializeField] public Image healthBarFill;
    [SerializeField] public TextMeshProUGUI healthMeterText;

    public void Update()
    {
        if (playerController != null)
        {
            UpdateHealthBar(playerController.currHP, playerController.maxHP);
        }

    }

    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = maxHealth > 0 ? currentHealth / (float)maxHealth : 0f;
        }


        if (healthMeterText != null)
        {
            healthMeterText.text = $"{currentHealth}/{maxHealth}";
        }

    }
}
