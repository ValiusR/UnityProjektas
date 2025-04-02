using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemySpawner;

public class FireBallController : WeaponController
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }



    public override void Attack()
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
        return $"Fires a ball of fire that deals {damage} damage per hit.";
    }
    public override string GetName()
    {
        return $"Fireball";
    }
}
