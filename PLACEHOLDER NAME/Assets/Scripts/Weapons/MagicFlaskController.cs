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

    protected override void Attack()
    {
        base.Attack();

        GameObject flask = Instantiate(prefab);
        flask.transform.position = gameObject.transform.position;

        MagicFlaskBehaviour stats = flask.GetComponent<MagicFlaskBehaviour>();
        stats.damage = this.damage;
        stats.speed = this.speed;
        stats.damageField = this.areaPrefab;
        
        //This / 2 is here, because SpriteRenderer.scale = 0.5 * Physics2D.Overlap()
        stats.areaSize = this.damageAreaSize;
        stats.damageSpeed = howFastEnemiesTakeDamage;
    }

    public override string GetDescription()
    {
        return $"Sends out a magic flask, that explodes in to a damaging zone.";
    }
    public override string GetName()
    {
        return $"Magic flask";
    }
}
