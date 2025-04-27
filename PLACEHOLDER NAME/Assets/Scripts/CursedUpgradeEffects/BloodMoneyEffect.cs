using UnityEngine;

public class BloodMoneyEffect : MonoBehaviour
{
    public int damageAmount = 50;

    public void ApplyBloodMoney(GameObject player)
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
    }
}