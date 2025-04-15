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
    [SerializeField] private LayerMask obstacleLayer;

    [SerializeField] public Vector2 playerInput;

    private Vector2 currentVelocity;

    private const float skinWidth = 0.01f;

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

        // Apply movement with collision checks
        AttemptMove(currentVelocity * Time.deltaTime);
    }

    void AttemptMove(Vector2 moveAmount)
    {
        Vector2 horizontalMove = new Vector2(moveAmount.x, 0);
        Vector2 verticalMove = new Vector2(0, moveAmount.y);

        // Horizontal Movement Check
        if (horizontalMove.magnitude > 0)
        {
            RaycastHit2D[] horizontalHits = new RaycastHit2D[1];
            int horizontalHitCount = rb.Cast(horizontalMove.normalized,
                new ContactFilter2D { layerMask = obstacleLayer, useLayerMask = true },
                horizontalHits,
                horizontalMove.magnitude + skinWidth);

            if (horizontalHitCount == 0)
            {
                rb.position += horizontalMove;
            }
        }

        // Vertical Movement Check
        if (verticalMove.magnitude > 0)
        {
            RaycastHit2D[] verticalHits = new RaycastHit2D[1];
            int verticalHitCount = rb.Cast(verticalMove.normalized,
                new ContactFilter2D { layerMask = obstacleLayer, useLayerMask = true },
                verticalHits,
                verticalMove.magnitude + skinWidth);

            if (verticalHitCount == 0)
            {
                rb.position += verticalMove;
            }
        }
    }
}
