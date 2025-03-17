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

    // Reference to the LevelUpUI (assign this in the inspector or find it dynamically)
   // public LevelUpUI levelUpUI;

    private void Start()
    {
        // Initialize the unlocked weapons list (e.g., with a starting weapon)
        unlockedWeapons = new List<WeaponController>();
        if (allWeapons.Count > 0)
        {
            unlockedWeapons.Add(allWeapons[0]); // Unlock the first weapon by default
        }
    }

    //paskui istrint reikes-------------------------
    private void Update()
    {
        if(experience >= experienceToNextLevel)
        {
            LevelUp();
        }
    }
    public static void GainXP(int amount)
    {
        experience += amount;
    }

    //----------------------------------------
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
        //experience -= experienceToNextLevel;
        experience = 0;
        experienceToNextLevel = CalculateExperienceToNextLevel();

        // Generate level-up options
        List<WeaponUpgradeOption> options = GenerateWeaponUpgradeOptions();


        levelUpUI.setWeaponUpgradeOptions(options);
        levelUpUI.ShowUI();

        // Show the level-up UI with the options
       // levelUpUI.ShowOptions(options);
    }

    private List<WeaponUpgradeOption> GenerateWeaponUpgradeOptions()
    {
        List<WeaponUpgradeOption> options = new List<WeaponUpgradeOption>();

        // Create a list of weapons that are not yet unlocked
        List<WeaponController> lockedWeapons = new List<WeaponController>(allWeapons);
        lockedWeapons.RemoveAll(weapon => unlockedWeapons.Contains(weapon));

        // Ensure we don't try to select more options than available weapons
        int numOptions = Mathf.Min(numberOfOptions, unlockedWeapons.Count + lockedWeapons.Count);

        // Shuffle the lists and pick the first 'numOptions' weapons
        List<WeaponController> shuffledUnlockedWeapons = new List<WeaponController>(unlockedWeapons);
        List<WeaponController> shuffledLockedWeapons = new List<WeaponController>(lockedWeapons);
        Shuffle(shuffledUnlockedWeapons);
        Shuffle(shuffledLockedWeapons);

        // Add upgrade options for unlocked weapons
        foreach (var weapon in shuffledUnlockedWeapons)
        {
            if (options.Count >= numOptions) break;

            string name = weapon.name;
            string description = $"Upgrade {weapon.name}: Increase damage by 10%";
            System.Action applyEffect = () => ApplyUpgrade(weapon);

            options.Add(new WeaponUpgradeOption(name, description, applyEffect));
        }

        // Add unlock options for locked weapons
        foreach (var weapon in shuffledLockedWeapons)
        {
            if (options.Count >= numOptions) break;

            string name = weapon.name;
            string description = $"Unlock {weapon.name}: {weapon.GetDescription()}";
            System.Action applyEffect = () => UnlockWeapon(weapon);

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

    private void UnlockWeapon(WeaponController weapon)
    {
        unlockedWeapons.Add(weapon);
        Debug.Log($"Unlocked {weapon.name}");
    }

    private int CalculateExperienceToNextLevel()
    {
        // Example formula for experience needed to reach the next level
        return 100 + (currentLevel * 50);
    }
}