using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightningController : WeaponController
{
    [Header("Lightning weapon stats")]
    public float lightningRange = 10f;
    public int howManyEnemiesToStrike;


    public override void EvolveWeapon(int evolutionLevel)
    {
        switch (evolutionLevel)
        {
            case 1:
                howManyEnemiesToStrike++;
                break;
            case 2:
                howManyEnemiesToStrike++;
                break;
            default:
                throw new InvalidOperationException("Maximum evolution level reached. Cannot evolve further.");
        }
    }

    public override string GetDescription()
    {
        return "Selects a random enemy and smites them!";
    }

    public override string GetEvolutionDescription(int evolutionLevel)
    {
        switch (evolutionLevel)
        {
            case 1:
                return $"Smites 2 enemies instead of 1";
            case 2:
                return $"Smites 3 enemies instead of 2";

            default:
                throw new InvalidOperationException("Maximum evolution level reached. Cannot evolve further.");
        }
    }

    public override string GetName()
    {
        return "Lightning";
    }

    protected override void Attack()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(this.transform.position, lightningRange);

        List<Collider2D> enemyColliders = new List<Collider2D>();

        foreach (Collider2D collider in hitColliders)
        {
            if (collider.GetComponent<EnemyHealthController>() != null)
            {
                enemyColliders.Add(collider);
            }
        }

        // Determine how many enemies to actually strike (up to the number of enemies available)
        int enemiesToStrike = Mathf.Min(howManyEnemiesToStrike, enemyColliders.Count);

        for (int i = 0; i < enemiesToStrike; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, enemyColliders.Count);
            Collider2D targetCollider = enemyColliders[randomIndex];

            enemyColliders.RemoveAt(randomIndex);

            StartCoroutine(PlayAttackAnimation(targetCollider.GetComponent<EnemyHealthController>()));
        }
    }

    private IEnumerator PlayAttackAnimation(EnemyHealthController enemy)
    {
        currCooldown = float.MaxValue;

        if (enemy != null)
        {
            GameObject lightning = Instantiate(this.prefab);
            lightning.transform.parent = enemy.gameObject.transform;
            lightning.transform.position = enemy.gameObject.transform.position;

            enemy.TakeDamage(this.damage);

            yield return new WaitForSeconds(0.32f);

            Destroy(lightning);
        }

        this.currCooldown = this.shootCooldown;
    }



    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        currCooldown -= Time.deltaTime;

        if (currCooldown <= 0)
        {
            Attack();
        }
    }
}
