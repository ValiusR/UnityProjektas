using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbBehaviour : BaseWeaponBehaviour
{
    public Transform pointToFollow;
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
        this.transform.RotateAround(pointToFollow.position, new Vector3(0, 0, 1), speed * 5 * Time.deltaTime);
    }

    protected override void OnCollisionWithEnemy(Collider2D collider)
    {
        base.OnCollisionWithEnemy(collider);
    }

    protected override void SolveCollisions()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(rb.position, collisionRadius);

        foreach (Collider2D hitCollider in hitColliders)
        {
            //Hit enemy, do damage
            if (hitCollider.CompareTag("Enemy"))
            {
                OnCollisionWithEnemy(hitCollider);
            }
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        
    }

   
}
