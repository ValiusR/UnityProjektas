using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemySpawner;

public class FireBallController : WeaponController
{
    [SerializeField] public bool tripleShot = false;
    [SerializeField] private float spreadAngle = 45f;
    [SerializeField] public bool doubleShot = false;
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    protected override void Start()
    {
        base.Start();
    }


    protected override void Attack()
    {
        base.Attack();

        // Get mouse position in world space
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Ensure z-position is consistent

        // Direction from player to mouse
        Vector2 baseDirection = (mousePos - transform.position).normalized;

        // Create center fireball
        if (tripleShot || !doubleShot)
        {
            CreateFireball(baseDirection);

        }
        audioManager.PlaySFX(audioManager.fireball);

        if (doubleShot)
        {
            // Calculate spread directions
            Vector2 leftDirection = Quaternion.Euler(0, 0, spreadAngle) * baseDirection;
            Vector2 rightDirection = Quaternion.Euler(0, 0, -spreadAngle) * baseDirection;

            CreateFireball(leftDirection);
            CreateFireball(rightDirection);
        }
    }

    private void CreateFireball(Vector2 direction)
    {
        GameObject fireball = Instantiate(prefab, transform.position, Quaternion.identity);
        FireBallBehaviour stats = fireball.GetComponent<FireBallBehaviour>();

        stats.damage = this.damage;
        stats.speed = this.speed;
        stats.SetDirection(direction); // This is crucial

        // Double-check the direction immediately
        Debug.Log($"Created fireball with direction: {direction}");
        Debug.DrawRay(transform.position, direction * 5, Color.red, 3f);
    }


    /*protected override void Attack()
    {
        base.Attack();

        GameObject fireball = Instantiate(prefab);
        fireball.transform.position = gameObject.transform.position;

        FireBallBehaviour stats = fireball.GetComponent<FireBallBehaviour>();
        stats.damage = this.damage;
        stats.speed = this.speed;
        if (tripleShot)
        {

        }
    }*/
    public override void EvolveWeapon(int evolutionLevel)
    {
        switch (evolutionLevel)
        {
            case 1:
                this.doubleShot = true;
                break;
            case 2:
                this.tripleShot = true;
                break;
            default:
                throw new InvalidOperationException("Maximum evolution level reached. Cannot evolve further.");
        }
       // this.tripleShot = true;

    }
    public override string GetEvolutionDescription(int evolutionLevel)
    {
        switch (evolutionLevel)
        {
            case 1:
                return $"Shoot two fireballs at 45° instead";
            case 2:
                return $"Shoot the fireball at the center position again";
               
            default:
                throw new InvalidOperationException("Maximum evolution level reached. Cannot evolve further.");
        }
       // return $"Tripple shot";
    }
    public override string GetDescription()
    {
        return $"Fires a ball of fire that deals {damage} damage per hit.";
    }
    public override string GetName()
    {
        return $"Fireball";
    }
    
    public override GameObject GetPrefab()
    {
        return prefab;
    }
    
}
