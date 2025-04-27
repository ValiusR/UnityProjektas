using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Movement")]
    [Range(0.1f, 20f)][SerializeField] public float maxSpeed = 5f;
    [Range(0.1f, 20f)][SerializeField] private float acceleration = 10f;
    [Range(0.1f, 20f)][SerializeField] private float deacceleration = 10f;
    [SerializeField] private LayerMask obstacleLayer;

    private Vector2 currentVelocity;
    private Vector2 targetInput;
    private Vector2 finalMove;
    public Vector2 PlayerInput => targetInput;

    [Header("Controls")]
    public static KeyCode moveUpKey = KeyCode.W;
    public static KeyCode moveDownKey = KeyCode.S;
    public static KeyCode moveLeftKey = KeyCode.A;
    public static KeyCode moveRightKey = KeyCode.D;

    private const string MOVE_UP_KEY = "MoveUpKey";
    private const string MOVE_DOWN_KEY = "MoveDownKey";
    private const string MOVE_LEFT_KEY = "MoveLeftKey";
    private const string MOVE_RIGHT_KEY = "MoveRightKey";

    void Awake()
    {
        LoadKeyBindings();
    }

    void Update()
    {
        // Input detection (not physics related)
        float horizontal = 0f;
        if (Input.GetKey(moveRightKey)) horizontal += 1f;
        if (Input.GetKey(moveLeftKey)) horizontal -= 1f;

        float vertical = 0f;
        if (Input.GetKey(moveUpKey)) vertical += 1f;
        if (Input.GetKey(moveDownKey)) vertical -= 1f;

        targetInput = new Vector2(horizontal, vertical).normalized;
    }

    void FixedUpdate()
    {
        // Smooth velocity calculation
        Vector2 targetVelocity = targetInput * maxSpeed;

        float usedAccel = (targetInput != Vector2.zero) ? acceleration : deacceleration;
        currentVelocity = Vector2.Lerp(currentVelocity, targetVelocity, usedAccel * Time.fixedDeltaTime);

        Vector2 moveAmount = currentVelocity * Time.fixedDeltaTime;
        AttemptMove(moveAmount);
    }

    void AttemptMove(Vector2 moveAmount)
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
