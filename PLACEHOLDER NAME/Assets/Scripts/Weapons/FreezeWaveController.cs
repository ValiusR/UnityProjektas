using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeWaveController : WeaponController
{
    [Header("Freeze weapon data")]
    [SerializeField] public float freezeLength;
    [SerializeField] public float freezeStrength;


    public override void Start()
    {
        base.Start();
    }



    public override void Attack()
    {
        base.Attack();

        GameObject wave = Instantiate(prefab);
        wave.transform.position = gameObject.transform.position;

        FreezeWaveBehaviour stats = wave.GetComponent<FreezeWaveBehaviour>();
        stats.damage = this.damage;
        stats.speed = this.speed;
        stats.freezeLength = this.freezeLength;
        stats.freezeStrength = this.freezeStrength;
    }

    public override string GetDescription()
    {
        return $"Deals {damage} damage per hit. Slows down hit enemies.";
    }
    public override string GetName()
    {
        return $"Freeze wave";
    }
}
