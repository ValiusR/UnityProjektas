using System.Collections.Generic;
using System.Linq;
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
    public int maxEvolutionLevel;

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
    /* private List<WeaponUpgradeOption> GenerateWeaponUpgradeOptions()
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
                     : () => ApplyUpgrade(weapon),
                 weapon.prefab
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
                 () => UnlockWeapon(weapon),
                 weapon.prefab
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
                         () => ApplyUpgrade(weapon),
                         weapon.prefab
                     ));
                 }
             }
         }

         Shuffle(options);
         return options;
     }*/
    private List<WeaponUpgradeOption> GenerateWeaponUpgradeOptions()
    {
        List<WeaponUpgradeOption> options = new List<WeaponUpgradeOption>();

        // Create weapon pools with null checks
        List<WeaponController> lockedWeapons = allWeapons
            .Where(w => w != null && !unlockedWeapons.Contains(w))
            .ToList();

        List<WeaponController> upgradableWeapons = unlockedWeapons
            .Where(w => w != null)
            .ToList();

        // Calculate slots - ensure at least 1 unlock shows if available
        int upgradeSlots = upgradableWeapons.Count > 0
            ? Mathf.Min(upgradableWeapons.Count, Mathf.CeilToInt(numberOfOptions / 2f))
            : 0;

        int unlockSlots = lockedWeapons.Count > 0
            ? Mathf.Min(lockedWeapons.Count, numberOfOptions - upgradeSlots)
            : 0;

        // Adjust if we can't fill slots
        if (options.Count < numberOfOptions && upgradeSlots + unlockSlots < numberOfOptions)
        {
            if (lockedWeapons.Count > 0)
            {
                unlockSlots = Mathf.Min(lockedWeapons.Count, numberOfOptions - upgradeSlots);
            }
            else
            {
                upgradeSlots = Mathf.Min(upgradableWeapons.Count, numberOfOptions);
            }
        }

        // Add upgrades
        foreach (var weapon in upgradableWeapons
            .OrderBy(w => w.weaponLevel) // Prioritize lower level weapons
            .ThenBy(w => Random.value)   // Then randomize
            .Take(upgradeSlots))
        {
            bool isEvolution = (weapon.weaponLevel > (weaponEvolutionInterval-1) &&
                weapon.weaponLevel % (weaponEvolutionInterval) == 0 && weapon.weaponLevel / (weaponEvolutionInterval) <= maxEvolutionLevel);

          //  bool isEvolution = weapon.weaponLevel == weaponEvolutionInterval - 1;
            int evolutionLevel = weapon.weaponLevel / (weaponEvolutionInterval);
            options.Add(new WeaponUpgradeOption(
                weapon.GetName(),
                isEvolution
                    ? $"EVOLUTION: {weapon.GetName()} - {weapon.GetEvolutionDescription(evolutionLevel)}"
                    : $"Upgrade {weapon.GetName()} (+10% damage)",
                isEvolution ? () => ApplyUpgradeAndEvolve(weapon, evolutionLevel) : () => ApplyUpgrade(weapon),
                weapon.prefab // Make sure prefab is assigned in inspector
            ));
        }

        // Add unlocks
        foreach (var weapon in lockedWeapons
            .OrderBy(w => Random.value)
            .Take(unlockSlots))
        {
            options.Add(new WeaponUpgradeOption(
                weapon.GetName(),
                $"NEW: {weapon.GetName()} - {weapon.GetDescription()}",
                () => UnlockWeapon(weapon),
                weapon.prefab
            ));
        }

        // Fill remaining slots with upgrades if needed
        if (options.Count < numberOfOptions)
        {
            var remainingUpgrades = upgradableWeapons
                .Where(w => !options.Any(o => o.name == w.GetName()))
                .OrderBy(w => Random.value)
                .Take(numberOfOptions - options.Count);

            foreach (var weapon in remainingUpgrades)
            {
                options.Add(new WeaponUpgradeOption(
                    weapon.GetName(),
                    $"Upgrade {weapon.GetName()} (+10% damage)",
                    () => ApplyUpgrade(weapon),
                    weapon.prefab
                ));
            }
        }

        // Final shuffle
        options = options.OrderBy(x => Random.value).ToList();
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
    private void ApplyUpgradeAndEvolve(WeaponController weapon, int evolutionLevel)
    {
        weapon.weaponLevel++;
        weapon.damage = (int)(weapon.damage * 1.1f); // Increase damage by 10%
        weapon.EvolveWeapon(evolutionLevel);
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