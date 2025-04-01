using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] DamageBlink damageBlink;
    [SerializeField] DeathUiManager deathUIManager; // Assign your Options UI Prefab in the Inspector
    public int maxHP;
    public int currHP;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void TakeDamage(int damage)
    {
        if(currHP > damage)
        {
            currHP -= damage;
            audioManager.PlaySFX(audioManager.playerHit);
        }
        else
        {
            currHP = 0;
        }

        //Play hurt animation
        damageBlink.PlayBlink();

        //Death
        if (currHP <= 0)
        {
            if (deathUIManager != null)
            {
                deathUIManager.ShowDeathUI();
            }
            else
            {
                SceneManager.LoadScene("MainMenu");
            }

        }
    }
}
