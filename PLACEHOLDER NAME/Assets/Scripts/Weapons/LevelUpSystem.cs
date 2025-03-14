using System.Collections.Generic;
using UnityEngine;

public class LevelUpSystem : MonoBehaviour
{
    public List<WeaponController> allWeapons; // List of all available weapons
    public int numberOfOptions = 3; // Number of options to present on level up

    private int currentLevel = 1;
    private int experience = 0;
    private int experienceToNextLevel = 100;

    // Reference to the LevelUpUI (you can assign this in the inspector or find it dynamically)
   // public LevelUpUI levelUpUI;

    public void GainExperience(int amount)
    {
        experience += amount;
        if (experience >= experienceToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentLevel++;
        experience -= experienceToNextLevel;
        experienceToNextLevel = CalculateExperienceToNextLevel();

        // Generate level-up options
        List<WeaponUpgradeOption> options = GenerateWeaponUpgradeOptions();

        // Show the level-up UI with the options
      //  levelUpUI.ShowOptions(options);
    }

    private List<WeaponUpgradeOption> GenerateWeaponUpgradeOptions()
    {
        List<WeaponUpgradeOption> options = new List<WeaponUpgradeOption>();

        // Ensure we don't try to select more options than available weapons
        int numOptions = Mathf.Min(numberOfOptions, allWeapons.Count);

        // Shuffle the list and pick the first 'numOptions' weapons
        List<WeaponController> shuffledWeapons = new List<WeaponController>(allWeapons);
        Shuffle(shuffledWeapons);

        for (int i = 0; i < numOptions; i++)
        {
            WeaponController weapon = shuffledWeapons[i];

            // Create an upgrade option for this weapon
            string name = weapon.name;
            string description = $"Increase {weapon.name}'s damage by 10%";
            System.Action applyEffect = () => ApplyUpgrade(weapon);

            options.Add(new WeaponUpgradeOption(name, description, applyEffect));
        }

        return options;
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    private void ApplyUpgrade(WeaponController weapon)
    {
        weapon.damage = (int)(weapon.damage * 1.1f); // Increase damage by 10%
        Debug.Log($"Upgraded {weapon.name} to {weapon.damage} damage");
    }

    private int CalculateExperienceToNextLevel()
    {
        // Example formula for experience needed to reach the next level
        return 100 + (currentLevel * 50);
    }
}