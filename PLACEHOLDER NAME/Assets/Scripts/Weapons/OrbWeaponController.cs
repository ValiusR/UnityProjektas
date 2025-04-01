using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbWeaponController : WeaponController
{

    private List<OrbBehaviour> orbs;

    [Header("Orb specific stats")]
    public int orbAmount;
    public float distanceFromPlayer;
    public float howFastEnemiesTakeDamage;

    public override void EvolveWeapon()
    {
        this.orbAmount+=2;
        this.CreateOrbs();
    }

    public override string GetDescription()
    {
        return "Magical orbs circle around, damaging enemies who dare to touch them";
    }

    public override string GetEvolutionDescription()
    {
        return "Get two extra balls";
    }

    public override string GetName()
    {
        return "Flying orbs";
    }
    public override GameObject GetPrefab()
    {
        return prefab;
    }

    protected override void Attack()
    {
        base.Attack();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        CreateOrbs();

    }

    private void CreateOrbs()
    {
        DeletePreviousOrbs();
        orbs = new List<OrbBehaviour>();

        float deg = CalculateAngle(orbAmount);

        for (int i = 0; i < orbAmount; i++)
        {
            OrbBehaviour orb = Instantiate(prefab).GetComponent<OrbBehaviour>();
            orb.transform.SetParent(this.transform);
            orb.transform.localPosition = new Vector3(Mathf.Sin(deg * i) * distanceFromPlayer, Mathf.Cos(deg * i) * distanceFromPlayer, 0);

            orb.pointToFollow = this.transform;
            orb.speed = this.speed;
            orb.damage = this.damage;
            orb.damageSpeed = this.howFastEnemiesTakeDamage;

            orbs.Add(orb);
        }
    }

    private void DeletePreviousOrbs()
    {
        if(orbs == null)
        {
            return;
        }

        for (int i = 0; i < orbs.Count; i++)
        {
            
            Destroy(orbs[i].gameObject);
        }
    }

    private float CalculateAngle(int amount)
    {
        return 360f / amount * Mathf.Deg2Rad;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            orbAmount++;
            CreateOrbs();
        }
    }

}
