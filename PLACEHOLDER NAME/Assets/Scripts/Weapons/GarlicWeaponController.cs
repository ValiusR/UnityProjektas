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
        Attack();
    }

    protected override void Attack()
    {
        GameObject zone = Instantiate(this.prefab);
        zone.transform.SetParent(this.transform);

        GarlicWeaponBehaviour stats = zone.GetComponent<GarlicWeaponBehaviour>();

        stats.damage = this.damage;
        stats.collisionRadius = this.damageAreaSize;
        stats.damageSpeed = howFastEnemiesTakeDamage;
        stats.transformToFollow = this.gameObject.transform;
    }

    protected override void Update()
    {
        // Empty override needed
    }
    public override string GetDescription()
    {
        return "Creates a zone around the player that damages surrounding enemies";
    }

    public override string GetName()
    {
        return "Garlic type weapon (PLACEHOLDER)";
    }
}
