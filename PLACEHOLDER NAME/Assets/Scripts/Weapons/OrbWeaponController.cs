using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrbWeaponController : WeaponController
{

    private List<OrbBehaviour> orbs;

    [Header("Orb specific stats")]
    public int orbAmount;
    public float distanceFromPlayer;
    public float howFastEnemiesTakeDamage;

    public override void EvolveWeapon(int evolutionLevel)
    {
        switch (evolutionLevel)
        {
            case 1:                
                    this.orbAmount += 2;
                    this.CreateOrbs();
                break;
            case 2:
                this.distanceFromPlayer += 1;
                this.CreateOrbs();
                break;
            default:
                throw new InvalidOperationException("Maximum evolution level reached. Cannot evolve further.");
        }
        //this.orbAmount+=2;
       // this.CreateOrbs();
    }

    public override string GetDescription()
    {
        return "Magical orbs circle around, damaging enemies who dare to touch them";
    }

    public override string GetEvolutionDescription(int evolutionLevel)
    {
       // return "Get two extra balls";
        switch (evolutionLevel)
        {
            case 1:
                return "Get two extra balls.";
            case 2:
                return "Balls are further from your character.";
            default:
                throw new InvalidOperationException("Maximum evolution level reached. Cannot evolve further.");
        }
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
