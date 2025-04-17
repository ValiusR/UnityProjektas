using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRangedProjectileBehaviour : BaseWeaponBehaviour
{
    public int Damage;

    // Start is called before the first frame update
    protected override void Start()
    {
        currDestroySeconds = destroyAfterSeconds;
    }

    protected override void FixedUpdate()
    {
        currDestroySeconds -= Time.fixedDeltaTime;
        if (currDestroySeconds < 0f)
        {
            Destroy(gameObject);
            return;
        }

        SolveCollisions();
    }

    protected override void SolveCollisions()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(rb.position, collisionRadius);

        foreach (Collider2D hitCollider in hitColliders)
        {
            PlayerHealthController player = hitCollider.GetComponent<PlayerHealthController>();

            if (player != null) 
            {
                player.TakeDamage(this.Damage);
                Destroy(this.gameObject);
            }
        }
    }

    
}
