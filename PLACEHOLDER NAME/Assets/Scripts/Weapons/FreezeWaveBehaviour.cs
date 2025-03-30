using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeWaveBehaviour : BaseWeaponBehaviour
{
    [Header("Freeze behaviour data")]
    [HideInInspector] public float freezeLength;
    [HideInInspector] public float freezeStrength;
    [SerializeField] public bool canPierce;

    public override void OnCollisionWithEnemy(Collider2D collider)
    {
        base.OnCollisionWithEnemy(collider);
        if (!canPierce)
        {
            Destroy(gameObject);
        }
        
        FreezeAnimationController freeze = collider.GetComponent<FreezeAnimationController>();

        if(freeze == null)
        {
            throw new Exception("Why is there no FreezeAnimationController on enemy?");
           // return;
        }

        freeze.ActivateFreeze(freezeLength, freezeStrength);

    }
}
