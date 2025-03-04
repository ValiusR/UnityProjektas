using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FireBallBehaviour : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;

    [Range(0f, 10f)]
    [SerializeField] float destroyAfterSeconds;
    private float currDestroySeconds;

    [Range(0f, 20f)]
    [SerializeField] float speed;

    // Start is called before the first frame update
    void Start()
    {
        currDestroySeconds = destroyAfterSeconds;

        Vector2 direction =  Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;

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
            Destroy(this);
        }

        //rb.velocity = new Vector2(1f,0f);
    }
}
