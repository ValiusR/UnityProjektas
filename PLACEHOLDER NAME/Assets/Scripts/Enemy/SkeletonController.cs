using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Transform player;
    [SerializeField] Animator am;
    [SerializeField] GameObject arrow;
    [SerializeField] Transform arrowPos;
    [SerializeField] EnemyMovement em;
    [SerializeField] public float attackSpeed;
    [SerializeField] public int damage;
    private float timer;


    // Start is called before the first frame update
    void Start()
    {
        em = GetComponent<EnemyMovement>();
        player = FindObjectOfType<PlayerMovementController>().transform;
        em.startMoveSpeed = em.moveSpeed;
        sr = GetComponent<SpriteRenderer>();
        am = GetComponent<Animator>();
        timer = attackSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // Constantly move the enemy towards the player
        if (Vector2.Distance(transform.position, player.transform.position) > 3 && am.GetBool("Shoot") == false)
        {
            am.SetBool("Move", true);
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, em.moveSpeed * Time.deltaTime);
        }
        else
        {
            am.SetBool("Move", false);
            am.SetBool("Shoot", true);
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = attackSpeed;
                Shoot();
                am.SetBool("Shoot", false);
            }
        }

        if (transform.position.x < player.transform.position.x)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }

    void Shoot()
    {
        Instantiate(arrow, arrowPos.position, Quaternion.identity);
    }

    
}
