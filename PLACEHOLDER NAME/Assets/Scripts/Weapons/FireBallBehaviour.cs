using UnityEngine;

public class FireBallBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    [Range(0f, 10f)]
    [SerializeField] private float destroyAfterSeconds;
    private float currDestroySeconds;

    [Range(0f, 20f)]
    [SerializeField] private float speed;

    [HideInInspector]
    public int damage;

    [SerializeField] private LayerMask propLayer;
    [SerializeField] private float collisionRadius = 0.1f;

    private Vector2 direction;

    void Start()
    {
        currDestroySeconds = destroyAfterSeconds;
        direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        direction = direction.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        rb.isKinematic = true;
    }

    void FixedUpdate()
    {
        currDestroySeconds -= Time.fixedDeltaTime;
        if (currDestroySeconds < 0f)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 movement = direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(rb.position, collisionRadius);

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (((1 << hitCollider.gameObject.layer) & propLayer) != 0)
            {
                Destroy(gameObject);
                break;
            }

            if (hitCollider.CompareTag("Enemy"))
            {
                EnemyHealthController enemyHealth = hitCollider.GetComponent<EnemyHealthController>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, collisionRadius);
    }
}