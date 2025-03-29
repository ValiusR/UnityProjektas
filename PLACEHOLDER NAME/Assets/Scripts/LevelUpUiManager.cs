using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
            Debug.Log("Option 1 Name: " + weaponUpgradeOptions[0].name);
            Debug.Log("Option 1 Description: " + weaponUpgradeOptions[0].description);
            button1Title.text = weaponUpgradeOptions[0].name;
            button1Description.text = weaponUpgradeOptions[0].description;
            button1Action = weaponUpgradeOptions[0].applyEffect;
        }
        if(weaponUpgradeOptions != null && weaponUpgradeOptions.Count >= 2)
        {
            Debug.Log("Option 2 Name: " + weaponUpgradeOptions[1].name);
            Debug.Log("Option 2 Description: " + weaponUpgradeOptions[1].description);
            button2Title.text = weaponUpgradeOptions[1].name;
            button2Description.text = weaponUpgradeOptions[1].description;
            button2Action = weaponUpgradeOptions[1].applyEffect;
        }
        if (weaponUpgradeOptions != null && weaponUpgradeOptions.Count >= 2)
        {
            Debug.Log("Option 3 Name: " + weaponUpgradeOptions[2].name);
            Debug.Log("Option 3 Description: " + weaponUpgradeOptions[2].description);
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
