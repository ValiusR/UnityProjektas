using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Component references
    [SerializeField] Rigidbody2D rb;


    // Movement
    [Range(0.1f, 20f)]
    [SerializeField] float maxSpeed;
    [Range(0.1f, 20f)]
    [SerializeField] float accelaration;
    [Range(0.1f, 20f)]
    [SerializeField] float deaccelaration;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerInput = new Vector2 ( Input.GetAxisRaw("Horizontal") , Input.GetAxisRaw("Vertical"));

        if (playerInput != new Vector2(0, 0))
        {
            MovePlayer(accelaration, playerInput);
        }
        else
        {
            MovePlayer(deaccelaration, playerInput);
        }
    }

    public void MovePlayer(float accel, Vector2 direction)
    {
        // The max speed that the player could reach (clamping needed, becuase moving diagonally adds extra speed)
        Vector2 targetVelocity = Vector2.ClampMagnitude(direction,1f) * maxSpeed;

        // Add the required accelaration to the current speed
        Vector2 moveVelocity = Vector2.Lerp(rb.velocity, targetVelocity, accel * Time.deltaTime);

        rb.velocity = new Vector2(moveVelocity.x, moveVelocity.y);
    }
}
