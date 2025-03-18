using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] public Transform player;
    [SerializeField] public float moveSpeed;
    [HideInInspector] public float startMoveSpeed;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        player = FindObjectOfType<PlayerMovementController>().transform;
        startMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // Constantly move the enemy towards the player
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
    }

    protected virtual void  OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            moveSpeed = 0;
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            moveSpeed = startMoveSpeed;
        }
    }
}
