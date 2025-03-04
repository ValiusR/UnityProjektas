using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Component references
    [Header("References")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] DamageBlink damageBlink;


    // Movement
    [Header("Movement")]
    [Range(0.1f, 20f)]
    [SerializeField] float maxSpeed;
    [Range(0.1f, 20f)]
    [SerializeField] float accelaration;
    [Range(0.1f, 20f)]
    [SerializeField] float deaccelaration;
    [SerializeField] public Vector2 playerInput;


    //Health
    [Header("Health")]
    public int maxHP;
    public int currHP;
   

    //Weapon data
    //[Header("Weapons")]


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (playerInput != new Vector2(0, 0))
        {
            MovePlayer(accelaration, playerInput);
        }
        else
        {
            MovePlayer(deaccelaration, playerInput);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(1);
        }
    }

    public void MovePlayer(float accel, Vector2 direction)
    {
        // The max speed that the player could reach (clamping needed, becuase moving diagonally adds extra speed)
        Vector2 targetVelocity = Vector2.ClampMagnitude(direction, 1f) * maxSpeed;

        // Add the required accelaration to the current speed
        Vector2 moveVelocity = Vector2.Lerp(rb.velocity, targetVelocity, accel * Time.deltaTime);

        rb.velocity = new Vector2(moveVelocity.x, moveVelocity.y);
    }

    public void TakeDamage(int damage)
    {
        currHP -= damage;

        //Play hurt animation
        damageBlink.PlayBlink();

        if(currHP <= 0) 
        { 
            // Game over

        }
    }
}
