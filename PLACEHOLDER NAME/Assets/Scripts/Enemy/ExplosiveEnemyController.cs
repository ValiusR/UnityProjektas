using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveEnemyController : MonoBehaviour
{
    [SerializeField] EnemyHealthController healthController;
    [SerializeField] float explosionRange;
    [SerializeField] int explosionDamage;
    private bool hasExploded = false;

    void Update()
    {
        if (!hasExploded && healthController.currHP <= 0)
        {
            SolveCollisions();
            hasExploded = true;
        }
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
}
