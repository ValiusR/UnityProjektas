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
    [SerializeField] float projectileSpreadAngle;

    [SerializeField] float meleeCooldown;
    [SerializeField] float rangedCooldown;
    private float lastMeleeTime = Mathf.NegativeInfinity;
    private float lastRangedTime = Mathf.NegativeInfinity;


    [SerializeField] GameObject rangedPrefab;

    private GameObject player;

    private Transform playerTransform;
    private PlayerHealthController healthController;

    private BossMovement BossMovement;

    private bool isAttacking = false;

    public enum BossAttackType
    {
        Melee,
        Ranged
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

        bool canMelee = Time.time >= lastMeleeTime + meleeCooldown;
        bool canRanged = Time.time >= lastRangedTime + rangedCooldown;

        if (distance < attackRange && canMelee)
        {
            isAttacking = true;
            lastMeleeTime = Time.time;
            StartCoroutine(HandleAttack(BossAttackType.Melee));
        }
        else if (canRanged)
        {
            isAttacking = true;
            lastRangedTime = Time.time;
            StartCoroutine(HandleAttack(BossAttackType.Ranged));
        }
    }

    IEnumerator HandleAttack(BossAttackType attackType)
    {

        switch (attackType)
        {
            case BossAttackType.Melee:
                yield return StartCoroutine(PlayMeleeAnimation());
                break;
            case BossAttackType.Ranged:
                yield return StartCoroutine(PlayRangedAnimation());
                break;
        }

        isAttacking = false; // Allow next attack
    }

    public IEnumerator PlayRangedAnimation()
    {
        Vector2 dir = playerTransform.position - this.gameObject.transform.position;

        CreateRangedProjectile(dir, 0);
        CreateRangedProjectile(dir, projectileSpreadAngle);
        CreateRangedProjectile(dir, -projectileSpreadAngle);


        yield return new WaitForSeconds(0.25f);
    }

    private void CreateRangedProjectile(Vector2 direction, float spreadAngle)
    {
        GameObject proj = Instantiate(rangedPrefab);
        proj.transform.position = this.gameObject.transform.position;

        // Rotate the direction vector by spreadAngle degrees
        Vector2 rotatedDir = RotateVector(direction.normalized, spreadAngle);

        // Set projectile rotation to match direction
        float angle = Mathf.Atan2(rotatedDir.y, rotatedDir.x) * Mathf.Rad2Deg;
        proj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Set projectile velocity
        proj.GetComponent<Rigidbody2D>().velocity = rotatedDir * 10;
    }

    private Vector2 RotateVector(Vector2 v, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        float tx = v.x;
        float ty = v.y;

        return new Vector2(
            tx * cos - ty * sin,
            tx * sin + ty * cos
        );
    }


    public IEnumerator PlayMeleeAnimation()
    {
        BossMovement.SetIsMoving(false);
        BossMovement.enabled = false;

        transform.DOJump(transform.position, 1.5f, 1, meleeJumpAnimationTime)
                     .SetEase(jumpCurve);

        yield return new WaitForSeconds(meleeJumpAnimationTime - 0.2f);

        PLACEHOLDER_damageZone.enabled = true;
        SolveCollisions();

        yield return new WaitForSeconds(0.2f);

        PLACEHOLDER_damageZone.enabled = false;
    }

    protected virtual void SolveCollisions()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(this.transform.position, attackRange);

        foreach (Collider2D hitCollider in hitColliders)
        {
            PlayerHealthController player = hitCollider.GetComponent<PlayerHealthController>();
            //Hit enemy, do damage
            if (player != null)
            {
                player.TakeDamage(5);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, attackRange);

    }
}


