using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicFlaskDamageAreaBehaviour : BaseWeaponBehaviour
{
    [HideInInspector] public float size;
     public float damageSpeed;

    private float currDamageSpeed;

    public override void Start()
    {
        currDestroySeconds = destroyAfterSeconds;
        currDamageSpeed = 0;
    }

    public override void FixedUpdate()
    {
        currDestroySeconds -= Time.fixedDeltaTime;
        if (currDestroySeconds < 0f)
        {
            Destroy(gameObject);
            return;
        }

        currDamageSpeed += Time.fixedDeltaTime;

        //Deal damage to every enemy that is in the area
        if(currDamageSpeed > damageSpeed)
        {
            SolveCollisions();
            currDamageSpeed = 0;
        }


    }

    public override void SolveCollisions()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, collisionRadius);

        foreach (Collider2D hitCollider in hitColliders)
        {
            //Hit enemy, do damage
            if (hitCollider.CompareTag("Enemy"))
            {
                OnCollisionWithEnemy(hitCollider);
            }
        }
    }

    public override void OnCollisionWithEnemy(Collider2D collider)
    {
        EnemyHealthController enemyHealth = collider.GetComponent<EnemyHealthController>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }
    }
}
