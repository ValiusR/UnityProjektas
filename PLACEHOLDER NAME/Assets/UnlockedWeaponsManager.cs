using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockedWeaponsUI : MonoBehaviour
{
    [SerializeField]
    HorizontalLayoutGroup layoutGroup;
    [SerializeField]
    LevelUpSystem levelUpSystem;

    // This script instance will hold the displayed weapon prefabs to prevent duplicates
    public HashSet<GameObject> displayedWeaponPrefabs = new HashSet<GameObject>();

    void Start()
    {
        if (layoutGroup == null)
        {
            Debug.LogError("HorizontalLayoutGroup not found on this GameObject!");
        }

        // Populate initial unlocked weapons (if needed)
        if (levelUpSystem != null && levelUpSystem.unlockedWeapons != null)
        {
            foreach (WeaponController wC in levelUpSystem.unlockedWeapons)
            {
                AddUnlockedWeaponToUI(wC.prefab);
            }
        }
    }

    public void AddUnlockedWeaponToUI(GameObject weaponPrefab)
    {
        // Check if this weapon prefab is already displayed
        if (displayedWeaponPrefabs.Contains(weaponPrefab))
        {
            return; // Don't add it again
        }

        // Create a new GameObject for the UI icon
        GameObject iconObject = new GameObject("WeaponIcon");
        iconObject.transform.SetParent(transform, false); // Set parent to the UI panel

        // Add an Image component to the new GameObject
        Image iconImage = iconObject.AddComponent<Image>();

        // Get the SpriteRenderer from the weapon prefab
        SpriteRenderer spriteRenderer = weaponPrefab.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            iconImage.sprite = spriteRenderer.sprite;

            // Optionally, you might want to set the size of the UI image
            // to match the sprite's aspect ratio or a fixed size.
            // Example:
            // float aspectRatio = (float)spriteRenderer.sprite.texture.width / spriteRenderer.sprite.texture.height;
            // iconImage.rectTransform.sizeDelta = new Vector2(50 * aspectRatio, 50);
            iconImage.preserveAspect = true; // Recommended to maintain aspect ratio
        }
        else
        {
            Debug.LogWarning($"No SpriteRenderer with a sprite found on prefab: {weaponPrefab.name} to set the UI icon.");
            Destroy(iconObject);
            return;
        }

        displayedWeaponPrefabs.Add(weaponPrefab);
    }
}