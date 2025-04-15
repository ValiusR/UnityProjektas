using UnityEngine;
using System.Collections; // Needed for Coroutine

public class PlayerMovementController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    // BoxCollider is not used in the provided movement logic, but kept if needed elsewhere
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Movement")]
    [Range(0.1f, 20f)][SerializeField] private float maxSpeed;
    [Range(0.1f, 20f)][SerializeField] private float acceleration;
    [Range(0.1f, 20f)][SerializeField] private float deacceleration;
    [SerializeField] private LayerMask obstacleLayer;

    private Vector2 currentVelocity;
    [HideInInspector] public Vector2 playerInput; // Hide from inspector, calculated internally

    [Header("Controls")]

    public static KeyCode moveUpKey = KeyCode.W;
    public static KeyCode moveDownKey = KeyCode.S;
    public static KeyCode moveLeftKey = KeyCode.A;
    public static KeyCode moveRightKey = KeyCode.D;

    // Key names for PlayerPrefs
    private const string MOVE_UP_KEY = "MoveUpKey";
    private const string MOVE_DOWN_KEY = "MoveDownKey";
    private const string MOVE_LEFT_KEY = "MoveLeftKey";
    private const string MOVE_RIGHT_KEY = "MoveRightKey";

    void Awake()
    {
        // Load saved keybindings or use defaults
        LoadKeyBindings();
    }

    void Update()
    {
        // Calculate input based on currently assigned keys
        float horizontalInput = 0f;
        if (Input.GetKey(moveRightKey)) horizontalInput += 1f;
        if (Input.GetKey(moveLeftKey)) horizontalInput -= 1f;

        float verticalInput = 0f;
        if (Input.GetKey(moveUpKey)) verticalInput += 1f;
        if (Input.GetKey(moveDownKey)) verticalInput -= 1f;

        // Create and normalize the input vector
        playerInput = new Vector2(horizontalInput, verticalInput).normalized;

        // Apply movement logic
        if (playerInput != Vector2.zero)
        {
            MovePlayer(acceleration, playerInput);
        }
        else
        {
            // Decelerate only if there's existing velocity and no input
            if (currentVelocity.magnitude > 0.01f)
            {
                MovePlayer(deacceleration, Vector2.zero);
            }
            else
            {
                currentVelocity = Vector2.zero; // Snap to zero if slow enough
                rb.velocity = Vector2.zero; 
            }
        }
    }

    void MovePlayer(float accel, Vector2 direction)
    {
        Vector2 targetVelocity = direction * maxSpeed;
        currentVelocity = Vector2.Lerp(currentVelocity, targetVelocity, accel * Time.deltaTime);

        AttemptMoveWithMovePosition(currentVelocity * Time.deltaTime);
    }

    void AttemptMoveWithMovePosition(Vector2 moveAmount)
    {
        Vector2 startPos = rb.position;
        Vector2 endPos = startPos + moveAmount;

        RaycastHit2D hit = Physics2D.BoxCast(startPos, boxCollider.size, 0f, moveAmount.normalized, moveAmount.magnitude, obstacleLayer);

        if (!hit)
        {
            rb.MovePosition(endPos);
        }
        else
        {
            currentVelocity = Vector2.zero; 
        }
    }
    public static void LoadKeyBindings()
    {
        moveUpKey = (KeyCode)PlayerPrefs.GetInt(MOVE_UP_KEY, (int)KeyCode.W);
        moveDownKey = (KeyCode)PlayerPrefs.GetInt(MOVE_DOWN_KEY, (int)KeyCode.S);
        moveLeftKey = (KeyCode)PlayerPrefs.GetInt(MOVE_LEFT_KEY, (int)KeyCode.A);
        moveRightKey = (KeyCode)PlayerPrefs.GetInt(MOVE_RIGHT_KEY, (int)KeyCode.D);
        Debug.Log("Player controls loaded.");
    }

}