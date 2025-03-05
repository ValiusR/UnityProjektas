using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] PlayerHealthController pc;
    [SerializeField] int damage;
    [SerializeField] float attackSpeed;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        pc = FindObjectOfType<PlayerHealthController>();
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                pc.TakeDamage(damage);
                timer = attackSpeed;
            }
        }
    }
}
