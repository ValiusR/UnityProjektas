using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject prefab;

    [Range(0f, 100f)]
    public int damage;

    [Range(0f, 100f)]
    [SerializeField] float speed;

    [Range(0f, 15)]
    [SerializeField] float shootCooldown;

    private float currCooldown;


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
    public virtual string GetDescription()
    {
        return $"Deals {damage} damage per hit.";
    }


}
