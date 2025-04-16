using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthController : EnemyHealthController
{
    private GameObject player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public override void TakeDamage(int damage)
    {
        currHP -= damage;
        damageBlink.PlayBlink();

        if (currHP <= 0)
        {
            StartCoroutine(PlayDeathAnimation());
            ScoreManager.addScore(scorePoints);
            LevelUpSystem.GainXP(xp);

            if (GetComponent<EnemyMovement>() != null)
            {
                GetComponent<EnemyMovement>().enabled = false;

                if (GetComponent<SkeletonController>() != null)
                {
                    GetComponent<SkeletonController>().enabled = false;
                    GetComponent<Animator>().enabled = false;
                }
            }

            GetComponent<Collider2D>().enabled = false;

            if (player.GetComponent<PlayerBoundary>() != null)
            {
                player.GetComponent<PlayerBoundary>().enabled = false;
            }

            // Trigger death event
            //OnDeath?.Invoke();
        }
    }
}
