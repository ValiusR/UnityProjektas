using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackManager : MonoBehaviour
{
    [SerializeField] float attackRange;
    [SerializeField] float meleeJumpAnimationTime;
    [SerializeField] SpriteRenderer PLACEHOLDER_damageZone;
    [SerializeField] AnimationCurve jumpCurve;

    private GameObject player;

    private Transform playerTransform;
    private PlayerHealthController healthController;

    private BossMovement BossMovement;

    private bool isAttacking = false;

    public enum BossAttackType
    {
        MeleeSlash,
        Fireball
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerTransform = player.transform;
        BossMovement = this.gameObject.GetComponent<BossMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking)
            return;

        float distance = Vector2.Distance(transform.position, playerTransform.position);

        BossMovement.enabled = true;
        BossMovement.SetIsMoving(true);

        if (distance < attackRange)
       {
            

            isAttacking = true;

            BossAttackType chosenAttack = BossAttackType.MeleeSlash;

            StartCoroutine(HandleAttack(chosenAttack));
       }

    }

    IEnumerator HandleAttack(BossAttackType attackType)
    {

        switch (attackType)
        {
            case BossAttackType.MeleeSlash:
                yield return StartCoroutine(PlayMeleeAnimation());
                break;
            case BossAttackType.Fireball:
                //yield return StartCoroutine(FireballAttack());
                break;
        }

        isAttacking = false; // Allow next attack
    }

    public IEnumerator PlayMeleeAnimation()
    {
        BossMovement.SetIsMoving(false);
        BossMovement.enabled = false;

        transform.DOJump(transform.position, 1.5f, 1, 1f)
                     .SetEase(jumpCurve);

        yield return new WaitForSeconds(meleeJumpAnimationTime);

        PLACEHOLDER_damageZone.enabled = true;

        yield return new WaitForSeconds(0.2f);

        PLACEHOLDER_damageZone.enabled = false;

        yield return new WaitForSeconds(0.2f);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, attackRange);

    }
}
