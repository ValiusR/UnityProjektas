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
    public int weaponEvolutionInterval = 2;

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
            int experienceToAdd = experience % experienceToNextLevel;
            LevelUp();
            experience += experienceToAdd;
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
            int experienceToAdd = experience % experienceToNextLevel;
            LevelUp();
            experience += experienceToAdd;
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

        
        List<WeaponController> lockedWeapons = new List<WeaponController>(allWeapons);
        lockedWeapons.RemoveAll(weapon => unlockedWeapons.Contains(weapon));

        List<WeaponController> upgradableWeapons = new List<WeaponController>(unlockedWeapons);

        // dabar puse upgrades, puse locked
        int maxUpgrades = Mathf.Min(upgradableWeapons.Count, Mathf.CeilToInt(numberOfOptions / 2f));
        int maxUnlocks = Mathf.Min(lockedWeapons.Count, numberOfOptions - maxUpgrades);

        //upgrades
        Shuffle(upgradableWeapons);
        for (int i = 0; i < upgradableWeapons.Count && options.Count < maxUpgrades; i++)
        {
            var weapon = upgradableWeapons[i];
            string description = weapon.weaponLevel == weaponEvolutionInterval - 1
                ? $"Upgrade {weapon.GetName()}: EVOLUTION AVAILABLE! {weapon.GetEvolutionDescription()}"
                : $"Upgrade {weapon.GetName()}: +10% damage";

            options.Add(new WeaponUpgradeOption(
                weapon.GetName(),
                description,
                weapon.weaponLevel == weaponEvolutionInterval - 1
                    ? () => ApplyUpgradeAndEvolve(weapon)
                    : () => ApplyUpgrade(weapon)
            ));
        }

        //unlocks
        Shuffle(lockedWeapons);
        for (int i = 0; i < lockedWeapons.Count && options.Count < numberOfOptions; i++)
        {
            var weapon = lockedWeapons[i];
            options.Add(new WeaponUpgradeOption(
                weapon.GetName(),
                $"Unlock {weapon.GetName()}: {weapon.GetDescription()}",
                () => UnlockWeapon(weapon)
            ));
        }

        
        if (options.Count < numberOfOptions)
        {
            for (int i = 0; i < upgradableWeapons.Count && options.Count < numberOfOptions; i++)
            {
                // Avoid duplicates
                if (!options.Exists(o => o.name == upgradableWeapons[i].GetName()))
                {
                    var weapon = upgradableWeapons[i];
                    options.Add(new WeaponUpgradeOption(
                        weapon.GetName(),
                        $"Upgrade {weapon.GetName()}: +10% damage",
                        () => ApplyUpgrade(weapon)
                    ));
                }
            }
        }

        Shuffle(options);
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
        weapon.weaponLevel++;
        weapon.damage = (int)(weapon.damage * 1.1f); // Increase damage by 10%
        Debug.Log($"Upgraded {weapon.name} to {weapon.damage} damage");
    }
    private void ApplyUpgradeAndEvolve(WeaponController weapon)
    {
        weapon.weaponLevel++;
        weapon.damage = (int)(weapon.damage * 1.1f); // Increase damage by 10%
        weapon.EvolveWeapon();
        Debug.Log($"EVOLVED {weapon.name} to {weapon.damage} damage");
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
        return 50 + currentLevel * 50;
    }
}