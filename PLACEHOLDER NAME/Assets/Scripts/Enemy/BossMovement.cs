using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : EnemyMovement
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) > 3)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
    }
}
