using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarlicWeaponBehaviour : BaseWeaponBehaviour
{
    [Header("Garlic behaviour data")]
    public float damageSpeed;

    private float currDamageSpeed;

    public Transform transformToFollow;

    protected override void FixedUpdate()
    {
        MoveProjectile();

        currDamageSpeed += Time.fixedDeltaTime;

        //Deal damage to every enemy that is in the area
        if (currDamageSpeed > damageSpeed)
        {
            SolveCollisions();
            currDamageSpeed = 0;
        }
    }

    protected override void MoveProjectile()
    {
        // MoveProjectile method not necessary
    }

    protected override void SolveCollisions()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, collisionRadius / 2);

        foreach (Collider2D hitCollider in hitColliders)
        {
            //Hit enemy, do damage
            if (hitCollider.CompareTag("Enemy"))
            {
                OnCollisionWithEnemy(hitCollider);
            }
        }
    }

    protected override void OnCollisionWithEnemy(Collider2D collider)
    {
        EnemyHealthController enemyHealth = collider.GetComponent<EnemyHealthController>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        currDestroySeconds = destroyAfterSeconds;
        currDamageSpeed = 0;

        this.transform.localScale = new Vector3(collisionRadius, collisionRadius, 1);

        SolveCollisions();
    }

    
}
