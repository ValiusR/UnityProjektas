using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicFlaskDamageAreaBehaviour : BaseWeaponBehaviour
{
    [Header("Magic flask behaviour data")]
    public float damageSpeed;

    private float currDamageSpeed;

    protected override void Start()
    {
        currDestroySeconds = destroyAfterSeconds;
        currDamageSpeed = 0;

        this.transform.localScale = new Vector3(collisionRadius, collisionRadius, 1);

        SolveCollisions();
    }

    protected override void FixedUpdate()
    {
        currDestroySeconds -= Time.fixedDeltaTime;
        if (currDestroySeconds < 0f)
        {
            StartCoroutine( PlayDeathAnimation());
            return;
        }

        currDamageSpeed += Time.fixedDeltaTime;

        //Deal damage to every enemy that is in the area
        if (currDamageSpeed > damageSpeed)
        {
            SolveCollisions();
            currDamageSpeed = 0;
        }


    }

    protected override void SolveCollisions()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, collisionRadius / 2);

        foreach (Collider2D hitCollider in hitColliders)
        {
            //Hit enemy, do damage
            if (hitCollider.CompareTag("Enemy") || hitCollider.GetComponent<EnemyHealthController>() != null)
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

    

    public IEnumerator PlayDeathAnimation()
    {
        yield return StartCoroutine(GetComponent<FadeOut>().FadeAnimation());

        Destroy(this.gameObject);

    }
}
