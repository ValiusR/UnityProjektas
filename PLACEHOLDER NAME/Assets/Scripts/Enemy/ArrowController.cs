using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public GameObject player;
    public Rigidbody2D rb;
    [SerializeField] public GameObject skeleton;
    public float speed;
    private float timer;
    // Start is called before the first frame update
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;

        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + -90);
    }

    // Update is called once per frame
    public void Update()
    {
        timer += Time.deltaTime;
        if (timer > 5)
        {
            DestroyImmediate(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealthController>().TakeDamage(skeleton.GetComponent<SkeletonController>().damage);
            Destroy(gameObject);
        }
    }

    public void SimulateCollisionEnter(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealthController playerHealth = other.GetComponent<PlayerHealthController>();
            SkeletonController skeletonController = skeleton.GetComponent<SkeletonController>();

            playerHealth.TakeDamage(skeletonController.damage);

            DestroyImmediate(gameObject); // Simulate arrow destruction
        }
    }
}
