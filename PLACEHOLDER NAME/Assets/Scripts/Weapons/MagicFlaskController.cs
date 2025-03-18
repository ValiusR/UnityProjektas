using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicFlaskController : WeaponController
{
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
