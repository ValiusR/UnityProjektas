using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FireBallBehaviour : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;

    [Range(0f, 10f)]
    [SerializeField] float destroyAfterSeconds;
    [SerializeField] float currDestroySeconds;

    [Range(0f, 20f)]
    [SerializeField] float speed;

    [HideInInspector]
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        currDestroySeconds = destroyAfterSeconds;

        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;

        direction = direction.normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Convert to degrees
        transform.rotation = Quaternion.Euler(0, 0, angle);

        rb.velocity = direction * speed;
    }

    // Update is called once per frame
    void Update()
    {
        currDestroySeconds -= Time.deltaTime;
        if (currDestroySeconds < 0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       

        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyHealthController>().TakeDamage(this.damage);

            Destroy(this.gameObject);
        }
    }
}
