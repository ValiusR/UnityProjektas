using System.Collections;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class EnemyHealthController : MonoBehaviour
{
    public event Action OnDeath; // New event

    [SerializeField] protected DamageBlink damageBlink;
    [SerializeField] protected FadeOut fadeOut;
    public LevelUpSystem levelUpSystem;
    public int maxHP;
    public int currHP;
    public int scorePoints;
    public int xp;
    private void Start()
    {
        // Find the first LevelUpSystem component in the scene
        levelUpSystem = FindObjectOfType<LevelUpSystem>();
        if (levelUpSystem == null)
        {
            Debug.LogError("No LevelUpSystem component found in the scene!");
            enabled = false;
        }
    }

    public virtual void TakeDamage(int damage)
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

            foreach (Transform child in transform)
            {
                if (child.name.Contains("Effect"))
                {
                    Destroy(child.gameObject);
                }
            }


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