using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    [SerializeField] DamageBlink damageBlink;

    public int maxHP;
    public int currHP;
    public int scorePoints;
    public void TakeDamage(int damage)
    {
        currHP -= damage;

        //Play hurt animation
        damageBlink.PlayBlink();

        if (currHP <= 0)
        {
            Destroy(this.gameObject);
            ScoreManager.addScore(scorePoints);
        }
    }
}
