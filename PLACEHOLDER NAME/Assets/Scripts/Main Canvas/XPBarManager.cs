using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class XPBarManager : MonoBehaviour
{
    [SerializeField] LevelUpSystem levelUpSystem; // Public field to assign the Player script
    [Header("Image")]
    [SerializeField] Image xpBarFill;
    [SerializeField] TextMeshProUGUI xpMeterText;

    void Start()
    {
        levelUpSystem = FindObjectOfType<LevelUpSystem>();
    }

    void Update()
    {
        UpdateXpBar(levelUpSystem.experience, levelUpSystem.experienceToNextLevel);
    }

    private void UpdateXpBar(int currentExperience, int experienceToNextLevel)
    {
        //Debug.Log("Current HEALTH: " + currentHealth);
        //Debug.Log("Max health: " + maxHealth);
        if (xpBarFill != null)
        {
            xpBarFill.fillAmount = currentExperience / (float)experienceToNextLevel;

        }
        if (xpMeterText != null)
        {
            xpMeterText.text = "XP: " + currentExperience + "/" + experienceToNextLevel;
        }
    }
}
