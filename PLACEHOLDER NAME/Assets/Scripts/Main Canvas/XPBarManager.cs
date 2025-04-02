using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPBarManager : MonoBehaviour
{
    [SerializeField] public LevelUpSystem levelUpSystem;
    [SerializeField] public Image xpBarFill;
    [SerializeField] public TextMeshProUGUI xpMeterText;

    public void Update()
    {
        if (levelUpSystem != null)
        {
            UpdateXpBar(LevelUpSystem.experience, levelUpSystem.experienceToNextLevel);
        }
    }

    public void UpdateXpBar(int currentExperience, int experienceToNextLevel)
    {
        if (xpBarFill != null)
        {
            xpBarFill.fillAmount = experienceToNextLevel > 0 ?
                Mathf.Clamp01(currentExperience / (float)experienceToNextLevel) : 0f;
        }

        if (xpMeterText != null)
        {
            xpMeterText.text = $"XP: {currentExperience}/{experienceToNextLevel}";
        }
    }
}