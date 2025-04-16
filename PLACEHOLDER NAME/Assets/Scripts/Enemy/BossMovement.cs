using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : EnemyMovement
{
    [SerializeField] Animator am;
    [SerializeField] SpriteRenderer sr;
    private bool isMoving = false;

    protected override void Start()
    {
        base.Start();
        am = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        am.SetBool("Move", true);
    }

    protected override void Update()
    {
        if (transform.position.x > player.transform.position.x && isMoving)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }


        //float distance = Vector2.Distance(transform.position, player.transform.position);

        //if (!isMoving && distance > 4.1f)
        //{
        //    isMoving = true;
        //    am.SetBool("Move", true);
        //}
        //else if (isMoving && distance < 3.9f)
        //{
        //    isMoving = false;
        //    am.SetBool("Move", false);
        //}

        isMoving = true;

        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
    }

    public void SetIsMoving(bool shouldMove)
    {
        am.SetBool("Move", shouldMove);
    }
}
