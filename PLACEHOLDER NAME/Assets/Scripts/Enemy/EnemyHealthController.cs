using System.Collections;
using UnityEngine;
using System;

public class EnemyHealthController : MonoBehaviour
{
    public event Action OnDeath; // New event

    [SerializeField] DamageBlink damageBlink;
    [SerializeField] FadeOut fadeOut;
    public int maxHP;
    public int currHP;
    public int scorePoints;
    public int xp;

    public void TakeDamage(int damage)
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

            // Trigger death event
            OnDeath?.Invoke();
        }
    }

    public IEnumerator PlayDeathAnimation()
    {
        yield return StartCoroutine(fadeOut.FadeAnimation());
        Destroy(gameObject);
    }
}