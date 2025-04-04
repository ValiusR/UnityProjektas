using UnityEngine;

public class FireBallBehaviour : BaseWeaponBehaviour
{
    // Add this to explicitly track direction
    private Vector2 _direction;

    protected override void Start()
    {
        base.Start();
        // Disable any physics interference
        rb.isKinematic = true;
        rb.gravityScale = 0;
    }

    public void SetDirection(Vector2 newDirection)
    {
        _direction = newDirection.normalized;

        // Update rotation to face direction
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    protected override void OnCollisionWithEnemy(Collider2D collider)
    {
        EnemyHealthController enemyHealth = collider.GetComponent<EnemyHealthController>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    protected override void MoveProjectile()
    {
        if (rb != null)
        {
            rb.MovePosition(rb.position + _direction * speed * Time.fixedDeltaTime);
        }
        else
        {
            transform.Translate(_direction * speed * Time.deltaTime, Space.World);
        }
    }
}