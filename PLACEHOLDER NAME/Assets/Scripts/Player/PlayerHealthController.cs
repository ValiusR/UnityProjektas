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
        currHP -= damage;

        //Play hurt animation
        damageBlink.PlayBlink();

        if (currHP <= 0)
        {
            // Game over

        }
    }
}
