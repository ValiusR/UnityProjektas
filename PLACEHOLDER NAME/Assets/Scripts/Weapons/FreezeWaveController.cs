using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemySpawner;

public class FreezeWaveController : WeaponController
{
    [Header("Freeze weapon data")]
    [SerializeField] public float freezeLength;
    [SerializeField] public float freezeStrength;
    [SerializeField] public bool canPierce;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void EvolveWeapon()
    {
        this.canPierce = true;

    }
    protected override void Attack()
    {
        base.Attack();

        GameObject wave = Instantiate(prefab);
        wave.transform.position = gameObject.transform.position;

        audioManager.PlaySFX(audioManager.freezeWave);

        FreezeWaveBehaviour stats = wave.GetComponent<FreezeWaveBehaviour>();
        stats.damage = this.damage;
        stats.speed = this.speed;
        stats.freezeLength = this.freezeLength;
        stats.freezeStrength = this.freezeStrength;
        stats.canPierce = this.canPierce;
    }

    public override string GetDescription()
    {
        return $"Deals {damage} damage per hit. Slows down hit enemies.";
    }
    public override string GetName()
    {
        return $"Freeze wave";
    }
    public override string GetEvolutionDescription()
    {
        return $"Pierces enemies.";
    }
    public override GameObject GetPrefab()
    {
        return prefab;
    }
}
