using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemDamage : EnemyDamage
{
    [SerializeField] public Animator am;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        am = GetComponent<Animator>();
        am.SetFloat("Speed", attackSpeed);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public void Attack()
    {
        pc.TakeDamage(damage);
;   }

    public override void OnCollisionStay2D(Collision2D collision)
    {
        
    }
}
