using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class GarlicWeaponController : WeaponController
{
    [Header("Garlic zone stats")]
    public float damageAreaSize;
    public float howFastEnemiesTakeDamage;



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
    public override void EvolveWeapon()
    {
        this.howFastEnemiesTakeDamage *= (float)0.8;

    }
    protected override void Attack()
    {
        GameObject zone = Instantiate(this.prefab);
        zone.transform.SetParent(this.transform);
        zone.transform.localPosition = new Vector3(0, 0, 0);

        GarlicWeaponBehaviour stats = zone.GetComponent<GarlicWeaponBehaviour>();

        stats.damage = this.damage;
        stats.collisionRadius = this.damageAreaSize;
        stats.damageSpeed = howFastEnemiesTakeDamage;
    }

    protected override void Update()
    {
        // Empty override needed
    }
    public override string GetDescription()
    {
        return "Creates a zone around the player that damages surrounding enemies";
    }
    public override string GetEvolutionDescription()
    {
        return $"Decreases the damage interval of garlic by 20%.";
    }
    public override string GetName()
    {
        return "Garlic";
    }
}
