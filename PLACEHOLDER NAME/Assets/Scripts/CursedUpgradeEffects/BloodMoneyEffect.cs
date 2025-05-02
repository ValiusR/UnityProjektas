using UnityEngine;
using UnityEngine.SceneManagement;

public class BloodMoneyEffect : MonoBehaviour
{
    public int damageAmount = 50;

    public GameObject deathBatPrefab; // Assign a bat prefab in inspector

    public void ApplyBloodMoney(GameObject player)
    {
        PlayerHealthController health = player.GetComponent<PlayerHealthController>();
        if (health == null) return;

        bool willDie = health.currHP <= damageAmount;
        health.TakeDamage(damageAmount);

        if (willDie)
        {
            // Spawn death bat right on player
            if (deathBatPrefab != null)
            {
                Instantiate(deathBatPrefab, player.transform.position, Quaternion.identity);
            }
            else
            {
                Debug.LogError("No death bat prefab assigned!");
            }

            // Still try to pause game
           // Time.timeScale = 0f;
            //health.GetComponent<PlayerMovementController>().enabled = false;
        }

        Destroy(this);
    }
    /*public void ApplyBloodMoney(GameObject player)
    {
        LevelUpSystem levelUp = player.GetComponent<LevelUpSystem>();
        PlayerHealthController healthController = player.GetComponent<PlayerHealthController>();

        if (levelUp != null)
        {
            levelUp.experienceToNextLevel /= 2;
            Debug.Log("Blood Money: Experience to next level halved.");
        }
        else
        {
            Debug.LogError("BloodMoney: LevelUpSystem component not found on the player.");
        }

        if (healthController != null)
        {
            healthController.TakeDamage(damageAmount);
            Debug.Log($"Blood Money: Player took {damageAmount} damage.");
        }
        else
        {
            Debug.LogError("BloodMoney: PlayerHealthController component not found on the player.");
        }

        // You might want to destroy this effect component after it's applied
        Destroy(this);
    }*/
}