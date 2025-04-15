using UnityEngine;

public class PlayerBoundary : MonoBehaviour
{
    [Header("Boundary Settings")]
    public float boundarySize = 20f;
    public float lineThickness = 0.5f;
    public Color lineColor = Color.black;

    [Header("Layer Settings")]
    public int boundaryLayer = 8; // Wall layer
    public int enemyLayer = 9; // Enemy layer

    private GameObject boundaryContainer;
    private GameObject player;
    private EnemyHealthController healthController;

    void Start()
    {
        player = GameObject.Find("Player");
        if (player == null)
        {
            Debug.LogError("Player object not found!");
            return;
        }

        healthController = GetComponent<EnemyHealthController>();
        if (healthController != null)
        {
            healthController.OnDeath += DestroyBoundary;
        }

        CreateBoundary();
    }

    void OnDestroy()
    {
        if (healthController != null)
        {
            healthController.OnDeath -= DestroyBoundary;
        }
    }

    void CreateBoundary()
    {
        boundaryContainer = new GameObject("BoundaryContainer");
        boundaryContainer.transform.position = player.transform.position;

        CreateBoundaryLine("Top", new Vector2(0, boundarySize / 2),
            new Vector2(boundarySize + lineThickness * 2, lineThickness));

        CreateBoundaryLine("Bottom", new Vector2(0, -boundarySize / 2),
            new Vector2(boundarySize + lineThickness * 2, lineThickness));

        CreateBoundaryLine("Left", new Vector2(-boundarySize / 2, 0),
            new Vector2(lineThickness, boundarySize + lineThickness * 2));

        CreateBoundaryLine("Right", new Vector2(boundarySize / 2, 0),
            new Vector2(lineThickness, boundarySize + lineThickness * 2));
    }

    void CreateBoundaryLine(string name, Vector2 position, Vector2 size)
    {
        GameObject line = new GameObject(name);
        line.transform.SetParent(boundaryContainer.transform);
        line.transform.localPosition = position;
        line.layer = boundaryLayer;

        // Visual component
        SpriteRenderer sr = line.AddComponent<SpriteRenderer>();
        sr.sprite = CreateLineSprite(size);
        sr.color = lineColor;
        sr.sortingOrder = 0;

        // Collision component
        BoxCollider2D col = line.AddComponent<BoxCollider2D>();
        col.size = size;

        // Physics component
        Rigidbody2D rb = line.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
    }

    void DestroyBoundary()
    {
        if (boundaryContainer != null)
        {
            Destroy(boundaryContainer);
        }
    }

    Sprite CreateLineSprite(Vector2 size)
    {
        int pixelWidth = Mathf.Max(1, Mathf.CeilToInt(size.x * 100));
        int pixelHeight = Mathf.Max(1, Mathf.CeilToInt(size.y * 100));

        Texture2D tex = new Texture2D(pixelWidth, pixelHeight);
        Color[] colors = new Color[pixelWidth * pixelHeight];
        for (int i = 0; i < colors.Length; i++) colors[i] = Color.white;
        tex.SetPixels(colors);
        tex.Apply();

        return Sprite.Create(tex, new Rect(0, 0, pixelWidth, pixelHeight), Vector2.one * 0.5f, 100);
    }
}