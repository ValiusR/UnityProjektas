using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] public PlayerHealthController pc;
    [SerializeField] public int damage;
    [SerializeField] public float attackSpeed;
    public float timer;

    // Start is called before the first frame update
    public virtual void Start()
    {
        pc = FindObjectOfType<PlayerHealthController>();
        timer = 0;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public virtual void OnCollisionStay2D(Collision2D collision)
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

    public void SimulateCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
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
