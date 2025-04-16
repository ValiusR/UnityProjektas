using UnityEngine;

public class BossHealthController : EnemyHealthController
{
    private PlayerBoundary playerBoundary;

    private void Start()
    {
        // Find the PlayerBoundary component on the player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerBoundary = player.GetComponent<PlayerBoundary>();
        }
    }

    public override void TakeDamage(int damage)
    {
        currHP -= damage;
        damageBlink.PlayBlink();

        if (currHP <= 0)
        {
            // Disable movement and collisions first
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

            // Remove the boundary when boss dies
            if (playerBoundary != null)
            {
                playerBoundary.DestroyBoundary();
            }

            // Handle score and XP
            ScoreManager.addScore(scorePoints);
            LevelUpSystem.GainXP(xp);

            // Play death animation
            StartCoroutine(PlayDeathAnimation());

            // Trigger any additional death events
           // OnDeath?.Invoke();
        }
    }
}