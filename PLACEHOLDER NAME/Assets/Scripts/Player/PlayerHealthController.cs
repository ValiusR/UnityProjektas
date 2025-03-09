using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] DamageBlink damageBlink;
    public int maxHP;
    public int currHP;
    public void TakeDamage(int damage)
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

        if (currHP <= 0)
        {
            // Game over

        }
    }
}
