using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthController : EnemyHealthController
{
    private GameObject player;
    private AudioManager audioManager;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public override void TakeDamage(int damage)
    {
        currHP -= damage;
        damageBlink.PlayBlink();

        if (currHP <= 0)
        {
            StartCoroutine(PlayDeathAnimation());
            ScoreManager.addScore(scorePoints);
            if (levelUpSystem != null)
            {
                levelUpSystem.GainXP(xp);
            }

            if (GetComponent<EnemyMovement>() != null)
            {
                GetComponent<EnemyMovement>().enabled = false;
            }

            if (GetComponent<SkeletonController>() != null)
            {
                GetComponent<SkeletonController>().enabled = false;
            }

            if (GetComponent<Animator>() != null)
            {
                GetComponent<Animator>().enabled = false;
            }

            if (GetComponent<Collider2D>() != null)
            {
                GetComponent<Collider2D>().enabled = false;
            }

            if (GetComponent<BossAttackManager>() != null)
            {
                GetComponent<BossAttackManager>().enabled = false;
            }

            if (player.GetComponent<PlayerBoundary>() != null)
            {
                player.GetComponent<PlayerBoundary>().enabled = false;
            }


            audioManager.ResumeBackgroundMusic();
            // Trigger death event
            //OnDeath?.Invoke();
        }
    }
}
