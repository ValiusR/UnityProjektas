using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LevelUpUiManager : MonoBehaviour
{
    [SerializeField] PlayerHealthController playerController;
    private List<WeaponUpgradeOption> weaponUpgradeOptions;
    private System.Action button1Action;
    private System.Action button2Action;
    private System.Action button3Action;

    [SerializeField] TextMeshProUGUI button1Title;
    [SerializeField] TextMeshProUGUI button1Description;
    [SerializeField] TextMeshProUGUI button2Title;
    [SerializeField] TextMeshProUGUI button2Description;
    [SerializeField] TextMeshProUGUI button3Title;
    [SerializeField] TextMeshProUGUI button3Description;
    [SerializeField] Image button1Image;
    [SerializeField] Image button2Image;
    [SerializeField] Image button3Image;

    private void Start()
    {
        HideUI();
    }

    public void Button1()
    {
        button1Action();
        HideUI();
    }
    public void Button2()
    {
        button2Action();
        HideUI();
    }
    public void Button3()
    {
        button3Action();
        HideUI();
    }
    public void Button4()
    {
        playerController.maxHP += 10;
        playerController.currHP = playerController.maxHP;
        HideUI();
    }

    public void HideUI()
    {

        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ShowUI()
    {
        if(weaponUpgradeOptions != null && weaponUpgradeOptions.Count >= 1)
        {
            SpriteRenderer spriteRenderer = weaponUpgradeOptions[0].imagePrefab.GetComponent<SpriteRenderer>();
            Color currentColor = button1Image.color;
            
            button1Image.sprite = spriteRenderer.sprite;
            currentColor.a = 1.0f;
            button1Title.text = weaponUpgradeOptions[0].name;
            button1Description.text = weaponUpgradeOptions[0].description;
            button1Action = weaponUpgradeOptions[0].applyEffect;
        }
        if(weaponUpgradeOptions != null && weaponUpgradeOptions.Count >= 2)
        {
            SpriteRenderer spriteRenderer = weaponUpgradeOptions[1].imagePrefab.GetComponent<SpriteRenderer>();
            Color currentColor = button2Image.color;
            
            button2Image.sprite = spriteRenderer.sprite;
            currentColor.a = 1.0f;
            button2Title.text = weaponUpgradeOptions[1].name;
            button2Description.text = weaponUpgradeOptions[1].description;
            button2Action = weaponUpgradeOptions[1].applyEffect;
        }
        if (weaponUpgradeOptions != null && weaponUpgradeOptions.Count >= 2)
        {
            SpriteRenderer spriteRenderer = weaponUpgradeOptions[2].imagePrefab.GetComponent<SpriteRenderer>();
            Color currentColor = button3Image.color;
            
            button3Image.sprite = spriteRenderer.sprite;
            currentColor.a = 1.0f;
            button3Title.text = weaponUpgradeOptions[2].name;
            button3Description.text = weaponUpgradeOptions[2].description;
            button3Action = weaponUpgradeOptions[2].applyEffect;
        }

        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void setWeaponUpgradeOptions(List<WeaponUpgradeOption> weaponUpgradeOptions)
    {
        this.weaponUpgradeOptions = weaponUpgradeOptions;
    }
}

public class GetSpriteFromGameObject : MonoBehaviour
{
    public GameObject targetGameObject; // Assign the GameObject in the Inspector

    private SpriteRenderer spriteRenderer;
    private Sprite currentSprite;

    void Start()
    {
        // Check if the target GameObject is assigned
        if (targetGameObject == null)
        {
            Debug.LogError("Target GameObject is not assigned in the Inspector!");
            return;
        }

        // Try to get the SpriteRenderer component from the target GameObject
        spriteRenderer = targetGameObject.GetComponent<SpriteRenderer>();

        // Check if a SpriteRenderer component exists
        if (spriteRenderer == null)
        {
            Debug.LogWarning("Target GameObject does not have a SpriteRenderer component.");
            return;
        }

        // Get the Sprite from the SpriteRenderer
        currentSprite = spriteRenderer.sprite;

        // Now you have access to the sprite in the 'currentSprite' variable
        if (currentSprite != null)
        {
            Debug.Log("Successfully retrieved sprite: " + currentSprite.name);
            // You can now use the 'currentSprite' variable for further operations
            // For example, you could access its properties like width, height, etc.
            Debug.Log("Sprite width: " + currentSprite.rect.width);
            Debug.Log("Sprite height: " + currentSprite.rect.height);
        }
        else
        {
            Debug.LogWarning("The SpriteRenderer on the target GameObject does not have a sprite assigned.");
        }
    }

    // You can create a public method to access the retrieved sprite from other scripts if needed
    public Sprite GetCurrentSprite()
    {
        return currentSprite;
    }
}
