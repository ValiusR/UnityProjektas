using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemDamage : EnemyDamage
{
    [SerializeField] Animator am;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        am = GetComponent<Animator>();
        am.SetFloat("Speed", attackSpeed);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    void Attack()
    {
        pc.TakeDamage(damage);
;    }

    private protected override void OnCollisionStay2D(Collision2D collision)
    {
        
    }
}
