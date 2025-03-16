using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemySpawner;

public class FireBallController : WeaponController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    

    protected override void Attack()
    {
        base.Attack();

        GameObject fireball = Instantiate(prefab);
        fireball.transform.position = gameObject.transform.position;

        FireBallBehaviour stats = fireball.GetComponent<FireBallBehaviour>();
        stats.damage = this.damage;
        stats.speed = this.speed;
    }

    public override string GetDescription()
    {
        return $"Deals {damage} damage per hit.";
    }
}
