using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeaponBehaviour : MonoBehaviour
{
    [Header("Base weapon behaviour stats")]
    [SerializeField] private Rigidbody2D rb;

    [Range(0f, 10f)]
    [SerializeField] private float destroyAfterSeconds;
    private float currDestroySeconds;

    [HideInInspector] 
    public float speed;

    [HideInInspector]
    public int damage;

    [SerializeField] private LayerMask propLayer;
    [SerializeField] private float collisionRadius = 0.1f;

    private Vector2 direction;

    public virtual void Start()
    {
        currDestroySeconds = destroyAfterSeconds;

        direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        direction = direction.normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        //rb.isKinematic = true;
    }

    public virtual void FixedUpdate()
    {
        currDestroySeconds -= Time.fixedDeltaTime;
        if (currDestroySeconds < 0f)
        {
            Destroy(gameObject);
            return;
        }


        MoveProjectile();
        SolveCollisions();
    }

    private bool DidHitProp(Collider2D hitCollider)
    {
        if (((1 << hitCollider.gameObject.layer) & propLayer) != 0)
        {
            return true;
        }

        return false;
    }

    public virtual void SolveCollisions()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(rb.position, collisionRadius);

        foreach (Collider2D hitCollider in hitColliders)
        {
            //Hit prop, get destroyed
            if (DidHitProp(hitCollider))
            {
                Destroy(gameObject);
                break;
            }

            //Hit enemy, do damage
            if (hitCollider.CompareTag("Enemy"))
            {
                OnCollisionWithEnemy(hitCollider);
            }
        }
    }

    public virtual void MoveProjectile()
    {
        Vector2 movement = direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    public virtual void OnCollisionWithEnemy(Collider2D collider)
    {
        EnemyHealthController enemyHealth = collider.GetComponent<EnemyHealthController>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, collisionRadius);
    }
}
