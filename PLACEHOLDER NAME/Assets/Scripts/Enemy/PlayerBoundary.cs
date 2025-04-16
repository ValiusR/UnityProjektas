using UnityEngine;

public class PlayerBoundary : MonoBehaviour
{
    [Header("Boundary Settings")]
    public float boundarySize = 20f;
    public float lineThickness = 0.5f;
    public Color lineColor = Color.black;

    [Header("Layer Settings")]
    public int boundaryLayer = 8;

    private GameObject boundaryContainer;
    private GameObject visualLinesContainer;
    private Transform playerTransform;
    private bool boundaryCreated = false;

    void Start()
    {
        playerTransform = transform;
    }

    public void CreateBoundaryOnBossSpawn()
    {
        if (boundaryCreated) return;

        // Create main container
        boundaryContainer = new GameObject("BoundaryColliders");
        boundaryContainer.transform.position = playerTransform.position;
        boundaryContainer.layer = boundaryLayer;

        // Setup static Rigidbody2D for physics interactions
        Rigidbody2D rb = boundaryContainer.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        float halfSize = boundarySize / 2f;

        // Create four Box Colliders (top, bottom, left, right)
        CreateBoxCollider("TopBoundary", new Vector2(0f, halfSize + lineThickness / 2f), new Vector2(boundarySize, lineThickness));
        CreateBoxCollider("BottomBoundary", new Vector2(0f, -halfSize + lineThickness / 2f), new Vector2(boundarySize, lineThickness));
        CreateBoxCollider("LeftBoundary", new Vector2(-halfSize - lineThickness / 2f, 0f), new Vector2(lineThickness, boundarySize));
        CreateBoxCollider("RightBoundary", new Vector2(halfSize + lineThickness / 2f, 0f), new Vector2(lineThickness, boundarySize));

        // Create visible boundaries
        CreateVisualBoundary();

        boundaryCreated = true;
    }

    void CreateBoxCollider(string name, Vector2 offset, Vector2 size)
    {
        GameObject boundaryPart = new GameObject(name);
        boundaryPart.transform.SetParent(boundaryContainer.transform);
        boundaryPart.transform.localPosition = offset;
        boundaryPart.layer = boundaryLayer;

        BoxCollider2D boxCollider = boundaryPart.AddComponent<BoxCollider2D>();
        boxCollider.size = size;
        boxCollider.isTrigger = false; // Ensure it's a solid collider
    }

    void CreateVisualBoundary()
    {
        visualLinesContainer = new GameObject("BoundaryVisuals");
        visualLinesContainer.transform.position = playerTransform.position;

        // Create four visible lines to match boundary
        CreateVisualLine("TopLine", new Vector2(0, boundarySize / 2),
            new Vector2(boundarySize, lineThickness));
        CreateVisualLine("BottomLine", new Vector2(0, -boundarySize / 2),
            new Vector2(boundarySize, lineThickness));
        CreateVisualLine("LeftLine", new Vector2(-boundarySize / 2, 0),
            new Vector2(lineThickness, boundarySize));
        CreateVisualLine("RightLine", new Vector2(boundarySize / 2, 0),
            new Vector2(lineThickness, boundarySize));
    }

    void CreateVisualLine(string name, Vector2 position, Vector2 size)
    {
        GameObject line = new GameObject(name);
        line.transform.SetParent(visualLinesContainer.transform);
        line.transform.localPosition = position;
        line.layer = boundaryLayer;

        // Create visible sprite
        SpriteRenderer sr = line.AddComponent<SpriteRenderer>();
        sr.sprite = CreateSolidSprite(size);
        sr.color = lineColor;
        sr.sortingOrder = 0;
    }

    Sprite CreateSolidSprite(Vector2 size)
    {
        int width = Mathf.CeilToInt(size.x * 100);
        int height = Mathf.CeilToInt(size.y * 100);
        Texture2D tex = new Texture2D(width, height);

        Color[] pixels = new Color[width * height];
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = Color.white;

        tex.SetPixels(pixels);
        tex.Apply();

        return Sprite.Create(tex, new Rect(0, 0, width, height),
            new Vector2(0.5f, 0.5f), 100f);
    }

    void OnDestroy()
    {
        if (boundaryContainer != null) Destroy(boundaryContainer);
        if (visualLinesContainer != null) Destroy(visualLinesContainer);
    }

    void OnDisable()
    {
        if (boundaryContainer != null) Destroy(boundaryContainer);
        if (visualLinesContainer != null) Destroy(visualLinesContainer);
    }
    // Add this to PlayerBoundary.cs
    public void DestroyBoundary()
    {
        if (boundaryContainer != null)
        {
            Destroy(boundaryContainer);
            boundaryContainer = null;
        }
        if (visualLinesContainer != null)
        {
            Destroy(visualLinesContainer);
            visualLinesContainer = null;
        }
        boundaryCreated = false;
    }
}