using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightningController : WeaponController
{
    public override void EvolveWeapon(int evolutionLevel)
    {
        switch (evolutionLevel)
        {
            case 1:
                //this.doubleShot = true;
                break;
            case 2:
                //this.tripleShot = true;
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
                return $" [INSERT_EVOLUTION_TEXT]";

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
        StartCoroutine(PlayAttackAnimation());
    }

    private IEnumerator PlayAttackAnimation()
    {
        currCooldown = float.MaxValue;

        float distanceFromplayer = 10f;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(this.transform.position, distanceFromplayer);
        int randomCollider = UnityEngine.Random.Range(0, hitColliders.Length);

        EnemyHealthController enemy = hitColliders[randomCollider].GetComponent<EnemyHealthController>();

        if(enemy == null)
        {
            this.currCooldown = 0;
            yield break;
        }

        GameObject lightning = Instantiate(this.prefab);
        lightning.transform.parent = enemy.gameObject.transform;
        lightning.transform.position = enemy.gameObject.transform.position;

        enemy.TakeDamage(this.damage);

        yield return new WaitForSeconds(0.32f);

        Destroy(lightning);

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
