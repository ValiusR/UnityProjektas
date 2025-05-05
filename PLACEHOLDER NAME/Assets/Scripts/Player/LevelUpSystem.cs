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
    private bool currentIsCursed = false;
    [Header("Cursed Upgrades")]
    [Range(0f, 1f)]
    public float cursedUpgradeChance = 0.2f; // Chance to offer a cursed upgrade
    public List<CursedUpgrade> possibleCursedUpgrades; // List of possible cursed upgrades
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

        levelUpUI.setWeaponUpgradeOptions(options, currentIsCursed);
        levelUpUI.ShowUI();
    }


    /*
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
        }*/

    private List<WeaponUpgradeOption> GenerateWeaponUpgradeOptions()
    {
        List<WeaponUpgradeOption> options = new List<WeaponUpgradeOption>();
        CursedUpgrade offeredCursedUpgrade = null; // To store the offered cursed upgrade

        // Chance for a cursed upgrade as the first option
        if (Random.value < cursedUpgradeChance && possibleCursedUpgrades.Count > 0)
        {
            currentIsCursed = true;
            offeredCursedUpgrade = possibleCursedUpgrades[Random.Range(0, possibleCursedUpgrades.Count)];
            options.Add(new WeaponUpgradeOption(
                offeredCursedUpgrade.name,
                offeredCursedUpgrade.description,
                () => ApplyCursedUpgrade(offeredCursedUpgrade),
                offeredCursedUpgrade.effectPrefab
            ));
        }
        else
        {
            currentIsCursed = false;
        }

            // Create weapon pools with null checks
            List<WeaponController> lockedWeapons = allWeapons
                .Where(w => w != null && !unlockedWeapons.Contains(w))
                .ToList();

        List<WeaponController> upgradableWeapons = unlockedWeapons
            .Where(w => w != null)
            .ToList();

        // Calculate slots for regular upgrades and unlocks
        int remainingOptionsSlots = numberOfOptions - options.Count;
        int upgradeSlots = Mathf.Min(upgradableWeapons.Count, Mathf.CeilToInt(remainingOptionsSlots / 2f));
        int unlockSlots = Mathf.Min(lockedWeapons.Count, remainingOptionsSlots - upgradeSlots);

        // Adjust if we can't fill slots
        if (options.Count < numberOfOptions && upgradeSlots + unlockSlots < remainingOptionsSlots)
        {
            if (lockedWeapons.Count > 0)
            {
                unlockSlots = Mathf.Min(lockedWeapons.Count, remainingOptionsSlots - upgradeSlots);
            }
            else
            {
                upgradeSlots = Mathf.Min(upgradableWeapons.Count, remainingOptionsSlots);
            }
        }

        // Add upgrades
        foreach (var weapon in upgradableWeapons
            .OrderBy(w => w.weaponLevel)
            .ThenBy(w => Random.value)
            .Take(upgradeSlots))
        {
            bool isEvolution = (weapon.weaponLevel > (weaponEvolutionInterval - 1) &&
                                weapon.weaponLevel % (weaponEvolutionInterval) == 0 &&
                                weapon.weaponLevel / (weaponEvolutionInterval) <= maxEvolutionLevel);
            int evolutionLevel = weapon.weaponLevel / (weaponEvolutionInterval);
            options.Add(new WeaponUpgradeOption(
                weapon.GetName(),
                isEvolution
                    ? $"EVOLUTION: {weapon.GetName()} - {weapon.GetEvolutionDescription(evolutionLevel)}"
                    : $"Upgrade {weapon.GetName()} (+10% damage)",
                isEvolution ? () => ApplyUpgradeAndEvolve(weapon, evolutionLevel) : () => ApplyUpgrade(weapon),
                weapon.prefab
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

        // Fill remaining slots with additional upgrades if needed
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

        // Shuffle the options, but exclude the first one if it's a cursed upgrade
        if (options.Count > 1 && offeredCursedUpgrade == null)
        {
            // Shuffle all options if no cursed upgrade was offered
            options = options.OrderBy(x => Random.value).ToList();
        }
        else if (options.Count > 1 && offeredCursedUpgrade != null)
        {
            // Shuffle all options *except* the first one (the cursed upgrade)
            var remainingOptions = options.Skip(1).OrderBy(x => Random.value).ToList();
            options = options.Take(1).Concat(remainingOptions).ToList();
        }

        return options;
    }
    private void ApplyCursedUpgrade(CursedUpgrade upgrade)
    {
        Debug.Log($"Applied cursed upgrade: {upgrade.name} - {upgrade.description}");
        upgrade.ApplyEffectEvent.Invoke(gameObject); // Invoke the UnityEvent
        // Optionally, you could add logic to track applied curses.
        possibleCursedUpgrades.Remove(upgrade);

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