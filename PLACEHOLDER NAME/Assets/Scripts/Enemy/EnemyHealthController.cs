using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    [SerializeField] DamageBlink damageBlink;
    [SerializeField] FadeOut fadeOut;

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
            StartCoroutine(PlayDeathAnimation());
            ScoreManager.addScore(scorePoints);

            GetComponent<EnemyMovement>().enabled = false;

            //PROBLEM, KOL PLAYINA ANIMATION, BATSAI VIS TIEK GALI PRADANGINTI PROJECTILES
        }
    }

    public IEnumerator PlayDeathAnimation()
    {
        yield return StartCoroutine(fadeOut.FadeAnimation());

        Destroy(this.gameObject);

    }
}
