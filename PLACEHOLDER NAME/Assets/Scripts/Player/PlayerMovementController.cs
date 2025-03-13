using UnityEngine;


public class PlayerMovementController : MonoBehaviour

{

    [Header("References")]

    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private BoxCollider2D boxCollider;


    [Header("Movement")]

    [Range(0.1f, 20f)][SerializeField] private float maxSpeed;

    [Range(0.1f, 20f)][SerializeField] private float acceleration;

    [Range(0.1f, 20f)][SerializeField] private float deacceleration;

    [SerializeField] private LayerMask obstacleLayer; // Add this to filter what the player collides with

    private Vector2 currentVelocity;

    [SerializeField] public Vector2 playerInput;


    void Update()

    {

        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;


        if (playerInput != Vector2.zero)

        {

            MovePlayer(acceleration, playerInput);

        }

        else

        {

            MovePlayer(deacceleration, Vector2.zero);

        }

    }


    void MovePlayer(float accel, Vector2 direction)

    {

        Vector2 targetVelocity = direction * maxSpeed;

        currentVelocity = Vector2.Lerp(currentVelocity, targetVelocity, accel * Time.deltaTime);


        // Try to move in that direction while checking for obstacles

        AttemptMove(currentVelocity * Time.deltaTime);

    }


    void AttemptMove(Vector2 moveAmount)
    {
        Vector2 horizontalMove = new Vector2(moveAmount.x, 0);
        Vector2 verticalMove = new Vector2(0, moveAmount.y);

        // Horizontal Movement Check
        if (horizontalMove.magnitude > 0) // Only check if there is horizontal movement.
        {
            RaycastHit2D[] horizontalHits = new RaycastHit2D[1];
            int horizontalHitCount = rb.Cast(horizontalMove.normalized, new ContactFilter2D { layerMask = obstacleLayer, useLayerMask = true }, horizontalHits, horizontalMove.magnitude);

            if (horizontalHitCount == 0)
            {
                rb.position += horizontalMove;
            }
        }

        // Vertical Movement Check
        if (verticalMove.magnitude > 0) // Only check if there is vertical movement.
        {
            RaycastHit2D[] verticalHits = new RaycastHit2D[1];
            int verticalHitCount = rb.Cast(verticalMove.normalized, new ContactFilter2D { layerMask = obstacleLayer, useLayerMask = true }, verticalHits, verticalMove.magnitude);

            if (verticalHitCount == 0)
            {
                rb.position += verticalMove;
            }
        }
    }

}