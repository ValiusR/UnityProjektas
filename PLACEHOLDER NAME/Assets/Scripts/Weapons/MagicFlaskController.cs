using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicFlaskController : WeaponController
{
    [Header("Magic flask properties")]
    [SerializeField] GameObject areaPrefab;
    [SerializeField] float damageAreaSize;
    [SerializeField] float howFastEnemiesTakeDamage;
    public bool doubledTime = false;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        currCooldown -= Time.deltaTime;

        if (currCooldown <= 0)
        {
            Attack();
        }
    }

    public override void EvolveWeapon(int evolutionLevel)
    {
        switch (evolutionLevel)
        {
            case 1:
                this.damageAreaSize *= (float)1.2;
                break;
            case 2:
                this.doubledTime = true;
                break;
            default:
                throw new InvalidOperationException("Maximum evolution level reached. Cannot evolve further.");
        }
        
         
       // this.howFastEnemiesTakeDamage = (float)(0.8 * this.howFastEnemiesTakeDamage);

    }
    protected override void Attack()
    {
        base.Attack();

        GameObject flask = Instantiate(prefab);
        flask.transform.position = gameObject.transform.position;

        MagicFlaskBehaviour stats = flask.GetComponent<MagicFlaskBehaviour>();
        stats.damage = this.damage;
        stats.speed = this.speed;
        stats.damageField = this.areaPrefab;
        if (doubledTime)
            stats.doubledTime = true;
        
        stats.areaSize = this.damageAreaSize;
        stats.damageSpeed = howFastEnemiesTakeDamage;
    }

    public override string GetDescription()
    {
        return $"Sends out a magic flask, that explodes in to a damaging zone.";
    }
    public override string GetEvolutionDescription(int evolutionLevel)
    {
        switch (evolutionLevel)
        {
            case 1:
                return $"Increases the zone radius by 20%.";
            case 2:
                return $"Doubles attack interval but also doubles time of the burn";
                break;
            default:
                throw new InvalidOperationException("Maximum evolution level reached. Cannot evolve further.");
        }
        //return $"Increases the zone radius by 20%.";
    }
    public override string GetName()
    {
        return $"Magic flask";
    }
    public override GameObject GetPrefab()
    {
        return prefab;
    }
}
