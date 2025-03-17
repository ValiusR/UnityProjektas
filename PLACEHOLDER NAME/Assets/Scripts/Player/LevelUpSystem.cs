using System.Collections.Generic;
using UnityEngine;

public class LevelUpSystem : MonoBehaviour
{
    public List<WeaponController> allWeapons; // List of all available weapons
    public List<WeaponController> unlockedWeapons; // List of weapons the player has unlocked
    public int numberOfOptions = 3; // Number of options to present on level up
    public LevelUpUiManager levelUpUI;
    public int currentLevel = 1;
    public static int experience = 0;
    public int experienceToNextLevel = 100;

    private void Start()
    {
        // Initialize the unlocked weapons list (e.g., with a starting weapon)
        unlockedWeapons = new List<WeaponController>();
        if (allWeapons.Count > 0)
        {
            unlockedWeapons.Add(allWeapons[0]); // Unlock the first weapon by default
            EnableWeaponBehavior(allWeapons[0]); // Enable the starting weapon's behavior
        }
    }

    private void Update()
    {
        if (experience >= experienceToNextLevel)
        {
            LevelUp();
        }
    }

    public static void GainXP(int amount)
    {
        experience += amount;
    }

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
        experience = 0;
        experienceToNextLevel = CalculateExperienceToNextLevel();

        // Generate level-up options
        List<WeaponUpgradeOption> options = GenerateWeaponUpgradeOptions();

        levelUpUI.setWeaponUpgradeOptions(options);
        levelUpUI.ShowUI();
    }

    private List<WeaponUpgradeOption> GenerateWeaponUpgradeOptions()
    {
        List<WeaponUpgradeOption> options = new List<WeaponUpgradeOption>();

        // Create a list of weapons that are not yet unlocked
        List<WeaponController> lockedWeapons = new List<WeaponController>(allWeapons);
        lockedWeapons.RemoveAll(weapon => unlockedWeapons.Contains(weapon));

        // Shuffle the locked weapons list
        Shuffle(lockedWeapons);

        // Add unlock options for locked weapons
        foreach (var weapon in lockedWeapons)
        {
            if (options.Count >= 2) break; // Limit to 2 options

            string name = weapon.name;
            string description = $"Unlock {weapon.GetName()}: {weapon.GetDescription()}";
            System.Action applyEffect = () => UnlockWeapon(weapon);

            options.Add(new WeaponUpgradeOption(name, description, applyEffect));
        }

        // If there are fewer than 2 options, add upgrade options for unlocked weapons
        if (options.Count < 2)
        {
            // Shuffle the unlocked weapons list
            List<WeaponController> shuffledUnlockedWeapons = new List<WeaponController>(unlockedWeapons);
            Shuffle(shuffledUnlockedWeapons);

            foreach (var weapon in shuffledUnlockedWeapons)
            {
                if (options.Count >= 2) break; // Limit to 2 options

                string name = weapon.name;
                string description = $"Upgrade {weapon.GetName()}: Increase damage by 10%";
                System.Action applyEffect = () => ApplyUpgrade(weapon);

                options.Add(new WeaponUpgradeOption(name, description, applyEffect));
            }
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

    private void UnlockWeapon(WeaponController weapon)
    {
        unlockedWeapons.Add(weapon);
        Debug.Log($"Unlocked {weapon.name}");

        // Enable the weapon's behavior script on the Player
        EnableWeaponBehavior(weapon);
    }

    private void EnableWeaponBehavior(WeaponController weapon)
    {
        // Get the weapon's behavior script type
        System.Type weaponBehaviorType = weapon.GetType();

        // Find the corresponding behavior script on the Player
        Component weaponBehavior = gameObject.GetComponent(weaponBehaviorType);

        if (weaponBehavior != null)
        {
            // Enable the behavior script
            if (weaponBehavior is MonoBehaviour)
            {
                ((MonoBehaviour)weaponBehavior).enabled = true;
                Debug.Log($"Enabled {weaponBehaviorType.Name} on Player");
            }
        }
        else
        {
            Debug.LogWarning($"Could not find {weaponBehaviorType.Name} on Player");
        }
    }

    private int CalculateExperienceToNextLevel()
    {
        // Example formula for experience needed to reach the next level
        return 100 + (currentLevel * 50);
    }
}