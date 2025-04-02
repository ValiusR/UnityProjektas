using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeAnimationController : MonoBehaviour
{
    [SerializeField] public Color freezeColor;

    public SpriteRenderer spriteRenderer;

    public EnemyMovement enemyMovement;

    public Coroutine freezeCoroutine;

    public void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyMovement = GetComponent<EnemyMovement>();
    }

    public void ActivateFreeze(float freezeTime, float freezeStrength)
    {
        if(freezeCoroutine != null)
        {
            //If enemy is rehit with a projectile, then the currFreezeTime should be resetted,
            //but too lazy rn
            return;
        }
        freezeCoroutine = StartCoroutine(ActivateFreezeCoroutine(freezeTime, freezeStrength));

    }

    public IEnumerator ActivateFreezeCoroutine(float freezeTime, float freezeStrength) 
    {
        float currTime = 0;

        //will be changed back after coroutine
        Color prevColor = spriteRenderer.color;

        spriteRenderer.color = freezeColor;

        float originalEnemySpeed = enemyMovement.moveSpeed;

        //Reduce the enemy speed
        enemyMovement.moveSpeed /= freezeStrength;

        while(currTime < freezeTime)
        {
            currTime += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = prevColor;

        //Bring back original speed
        enemyMovement.moveSpeed = originalEnemySpeed;

        yield return null;
    }

    
}
