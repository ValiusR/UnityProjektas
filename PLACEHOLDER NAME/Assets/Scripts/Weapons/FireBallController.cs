using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        fireball.GetComponent<FireBallBehaviour>().damage = this.damage;
    }
}
