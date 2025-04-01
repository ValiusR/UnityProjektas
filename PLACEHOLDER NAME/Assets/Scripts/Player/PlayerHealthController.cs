using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] public DamageBlink damageBlink;
    [SerializeField] public DeathUiManager deathUIManager; // Assign your Options UI Prefab in the Inspector
    public int maxHP;
    public int currHP;

    private void Awake()
    {
        Debug.Log("PlayerHealthController Awake. damageBlink: " + (damageBlink != null));
    }
    public virtual void TakeDamage(int damage)
    {
        if(currHP > damage)
        {
            currHP -= damage;
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
