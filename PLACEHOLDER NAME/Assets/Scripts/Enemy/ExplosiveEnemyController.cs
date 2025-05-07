using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveEnemyController : MonoBehaviour
{
    [SerializeField] EnemyHealthController healthController;
    [SerializeField] float explosionRange;
    [SerializeField] int explosionDamage;

    [SerializeField] GameObject explosion;

    void Start()
    {
        // Subscribe to the OnDeath event of the EnemyHealthController
        if (healthController != null)
        {
            healthController.OnDeath += HandleOnDeath;
        }
        else
        {
            Debug.LogError("EnemyHealthController is not assigned in the Inspector for " + gameObject.name);
        }
    }

    // This function will be called when the OnDeath event is invoked
    private void HandleOnDeath()
    {
        MakeExplosion();
        SolveCollisions();
    }

    private void MakeExplosion()
    {
        GameObject expl = Instantiate(explosion, this.transform.position, Quaternion.identity);
    }

    protected virtual void SolveCollisions()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(this.transform.position, explosionRange);

        foreach (Collider2D hitCollider in hitColliders)
        {
            PlayerHealthController player = hitCollider.GetComponent<PlayerHealthController>();
            //Hit enemy, do damage
            if (player != null)
            {
                player.TakeDamage(explosionDamage);
            }
        }
    }

    // It's good practice to unsubscribe from events when the object is destroyed
    private void OnDestroy()
    {
        if (healthController != null)
        {
            healthController.OnDeath -= HandleOnDeath;
        }
    }
}