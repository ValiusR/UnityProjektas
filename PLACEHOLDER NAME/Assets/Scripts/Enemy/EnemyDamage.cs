using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] PlayerHealthController pc;
    [SerializeField] public int damage;
    [SerializeField] public float attackSpeed;
    protected float timer;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        pc = FindObjectOfType<PlayerHealthController>();
        timer = 0;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected virtual private void OnCollisionStay2D(Collision2D collision)
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
