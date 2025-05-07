using System;
using UnityEngine;

[System.Serializable]
public class WeaponUpgradeOption
{
    public string name; // Name of the weapon
    public string description; // Description of the upgrade
    public System.Action applyEffect; // Action to apply the upgrade effect
    public GameObject imagePrefab;
    public bool isCursed;

    public WeaponUpgradeOption(string name, string description, System.Action applyEffect)
    {
        this.name = name;
        this.description = description;
        this.applyEffect = applyEffect;
    }
    public WeaponUpgradeOption(string name, string description, System.Action applyEffect, GameObject imagePrefab)
    {
        this.name = name;
        this.description = description;
        this.applyEffect = applyEffect;
        this.imagePrefab = imagePrefab;
        this.isCursed = false;
    }

    public WeaponUpgradeOption(string name, string description, System.Action applyEffect, GameObject imagePrefab, bool isCursed)
    {
        this.name = name;
        this.description = description;
        this.applyEffect = applyEffect;
        this.imagePrefab = imagePrefab;
        this.isCursed = isCursed;
    }
}