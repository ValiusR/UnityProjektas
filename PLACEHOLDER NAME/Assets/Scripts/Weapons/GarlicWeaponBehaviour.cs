using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GarlicWeaponBehaviour : BaseWeaponBehaviour
{
    [Header("Garlic behaviour data")]
    public float damageSpeed;

    private float currDamageSpeed;

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

    // Start is called before the first frame update
    protected override void Start()
    {
        
            transform?.DORotate(new Vector3(0f, 0f, 360f), 20f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);

            currDestroySeconds = destroyAfterSeconds;
            currDamageSpeed = 0;

            SolveCollisions();

    }

    public void SetScale()
    {
        this.transform.localScale = new Vector3(collisionRadius, collisionRadius, 1);
    }


}
