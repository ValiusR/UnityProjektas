using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class GarlicWeaponController : WeaponController
{
    [Header("Garlic zone stats")]
    public float damageAreaSize;
    private GameObject currentZone;
    public float howFastEnemiesTakeDamage;
   // private GarlicWeaponBehaviour stats = null;



    protected override void Start()
    {
        // Empty start needed
    }

    private void OnEnable()
    {
        // When the garlicController is enabled, then
        // spawn the damage zone
        Attack();
    }
    public override void EvolveWeapon(int evolutionLevel)
    {
        switch (evolutionLevel)
        {
            case 1:
                this.howFastEnemiesTakeDamage *= (float)0.8;
                break;
            case 2:
                this.DeleteZoneFromParent();
                this.damageAreaSize += 2;
                
               // stats = null;
                this.Attack();
                break;
            default:
                throw new InvalidOperationException("Maximum evolution level reached. Cannot evolve further.");
        }
      //  this.howFastEnemiesTakeDamage *= (float)0.8;

    }
    protected override void Attack()
    {
       
        currentZone = Instantiate(this.prefab);
        currentZone.transform.SetParent(this.transform);
        // -0.37f so that the garlic looks more centered
        currentZone.transform.localPosition = new Vector3(0, -0.3f, 0);

        GarlicWeaponBehaviour stats = currentZone.GetComponent<GarlicWeaponBehaviour>();

        stats.damage = this.damage;
        stats.collisionRadius = this.damageAreaSize;
        stats.damageSpeed = howFastEnemiesTakeDamage;

        //Needed because the scale is the size of the zone visually
        stats.SetScale();
    }

    protected override void Update()
    {
        // Empty override needed
    }
    private void DeleteZoneFromParent()
    {
        if (currentZone != null)
        {
            DG.Tweening.DOTween.Clear(currentZone);
           
            Destroy(currentZone); // Destroy the zone
            currentZone = null;
        }
        else
        {
            Debug.LogWarning("No zone to delete.");
        }
    }
    public override string GetDescription()
    {
        return "Creates a zone around the player that damages surrounding enemies";
    }
    public override string GetEvolutionDescription(int evolutionLevel)
    {
        switch (evolutionLevel)
        {
            case 1:
                return "Decreases the damage interval of magic aura by 20%.";
            case 2:
                return "Extra radius.";
            default:
                throw new InvalidOperationException("Maximum evolution level reached. Cannot evolve further.");
        }
       // return $"Decreases the damage interval of garlic by 20%.";
    }
    public override string GetName()
    {
        return "Magic aura";
    }
    public override GameObject GetPrefab()
    {
        return prefab;
    }
}
