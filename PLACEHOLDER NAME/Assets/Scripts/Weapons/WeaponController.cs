using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Base weapon data")]

    public GameObject prefab;

    [Range(0f, 100f)]
    public int damage;

    [Range(0f, 100f)]
    public float speed;

    [Range(0f, 15)]
    public float shootCooldown;

    private float currCooldown;
    public int weaponLevel=1;


    protected virtual void Start()
    {
        currCooldown = 0;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        currCooldown -= Time.deltaTime;

        if (currCooldown <= 0 && Input.GetKey(KeyCode.Mouse0))
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        currCooldown = shootCooldown;
    }
    public virtual void EvolveWeapon()
    {
        throw new UnassignedReferenceException(" Weapon() method is run for base weapon class ");
    }
    public virtual string GetDescription()
    {
        throw new UnassignedReferenceException(" GetDescription() method is run for base weapon class ");

    }
    public virtual string GetName()
    {
        throw new UnassignedReferenceException(" GetName() method is run for base weapon class ");

    }
    public virtual string GetEvolutionDescription()
    {
        throw new UnassignedReferenceException(" GetEvolutionDescription() method is run for base weapon class ");

    }

}
