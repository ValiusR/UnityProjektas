using UnityEngine;
using UnityEngine.Events;

public class MachineGunCurseEffect : MonoBehaviour
{
    private float originalShootCooldownFireball;
    private float originalShootCooldownFreezeWave;
    private float originalMaxSpeed;
    private FireBallController fireballController;
    private FreezeWaveController freezeWaveController;
    private PlayerMovementController playerMovementController; // Corrected variable name
    private bool effectApplied = false;

    public void ApplyMachineGunCurse(GameObject player)
    {
        if (effectApplied) return;

        fireballController = player.GetComponent<FireBallController>();
        freezeWaveController = player.GetComponent<FreezeWaveController>();
        playerMovementController = player.GetComponent<PlayerMovementController>(); // Get the correct component

        if (fireballController != null)
        {
            originalShootCooldownFireball = fireballController.shootCooldown;
            fireballController.shootCooldown *= 0.5f;
            Debug.Log("Machine Gun Curse: Fireball shoot cooldown halved to " + fireballController.shootCooldown);
        }
        else
        {
            Debug.LogWarning("Machine Gun Curse: FireBallController not found on the player.");
        }

        if (freezeWaveController != null)
        {
            originalShootCooldownFreezeWave = freezeWaveController.shootCooldown;
            freezeWaveController.shootCooldown *= 0.5f;
            Debug.Log("Machine Gun Curse: Freeze Wave shoot cooldown halved to " + freezeWaveController.shootCooldown);
        }
        else
        {
            Debug.LogWarning("Machine Gun Curse: FreezeWaveController not found on the player.");
        }

        if (playerMovementController != null)
        {
            originalMaxSpeed = playerMovementController.maxSpeed;
            playerMovementController.maxSpeed *= 0.5f;
            Debug.Log("Machine Gun Curse: Player max speed halved to " + playerMovementController.maxSpeed);
        }
        else
        {
            Debug.LogWarning("Machine Gun Curse: PlayerMovementController script not found on the player.");
        }

        effectApplied = true;
        Destroy(this); // Apply once and remove the effect script
    }

    // Optional: Implement a method to revert the changes if the curse is temporary
    public void RevertMachineGunCurse(GameObject player)
    {
        if (effectApplied)
        {
            FireBallController fbController = player.GetComponent<FireBallController>();
            FreezeWaveController fwController = player.GetComponent<FreezeWaveController>();
            PlayerMovementController pmController = player.GetComponent<PlayerMovementController>(); // Corrected variable name

            if (fbController != null)
            {
                fbController.shootCooldown = originalShootCooldownFireball;
                Debug.Log("Machine Gun Curse: Fireball shoot cooldown reverted to " + fbController.shootCooldown);
            }

            if (fwController != null)
            {
                fwController.shootCooldown = originalShootCooldownFreezeWave;
                Debug.Log("Machine Gun Curse: Freeze Wave shoot cooldown reverted to " + fwController.shootCooldown);
            }

            if (pmController != null)
            {
                pmController.maxSpeed = originalMaxSpeed;
                Debug.Log("Machine Gun Curse: Player max speed reverted to " + pmController.maxSpeed);
            }
            effectApplied = false;
        }
    }
}