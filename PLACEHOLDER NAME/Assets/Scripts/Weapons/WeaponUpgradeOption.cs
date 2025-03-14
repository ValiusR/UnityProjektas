using UnityEngine;

[System.Serializable]
public class WeaponUpgradeOption
{
    public string name; // Name of the weapon
    public string description; // Description of the upgrade
    public System.Action applyEffect; // Action to apply the upgrade effect

    public WeaponUpgradeOption(string name, string description, System.Action applyEffect)
    {
        this.name = name;
        this.description = description;
        this.applyEffect = applyEffect;
    }
}