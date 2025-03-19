using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAnimation : MonoBehaviour
{
    [SerializeField] Animator am;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Transform player;
    // Start is called before the first frame update
    void Start()
    {
        am = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerMovementController>().transform;
        am.SetBool("Move", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < player.transform.position.x)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            am.SetBool("Move", false);
            am.SetBool("Attack", true);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            am.SetBool("Attack", false);
            am.SetBool("Move", true);
        }
    }
}
