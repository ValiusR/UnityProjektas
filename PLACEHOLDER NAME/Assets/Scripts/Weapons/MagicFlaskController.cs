using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicFlaskController : WeaponController
{
    [Header("Magic flask properties")]
    [SerializeField] GameObject areaPrefab;
    [SerializeField] float damageAreaSize;
    [SerializeField] float howFastEnemiesTakeDamage;

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

    public override void EvolveWeapon()
    {
         this.damageAreaSize *= (float)1.2;
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
        
        stats.areaSize = this.damageAreaSize;
        stats.damageSpeed = howFastEnemiesTakeDamage;
    }

    public override string GetDescription()
    {
        return $"Sends out a magic flask, that explodes in to a damaging zone.";
    }
    public override string GetEvolutionDescription()
    {
        return $"Increases the zone radius by 20%.";
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
