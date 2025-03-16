using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] public float moveSpeed;
    private float startMoveSpeed;


    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovementController>().transform;
        startMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // Constantly move the enemy towards the player
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            moveSpeed = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            moveSpeed = startMoveSpeed;
        }
    }
}
