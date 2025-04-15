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
    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
        if (player == null)
        {
            Debug.LogError("Player object not found!");
            return;
        }

        CreateBoundary();
    }

    void CreateBoundary()
    {

        // Create main container
        boundaryContainer = new GameObject("BoundaryEdges");
        boundaryContainer.transform.position = player.transform.position;
        boundaryContainer.layer = boundaryLayer;

        // Setup static Rigidbody2D for physics interactions
        Rigidbody2D rb = boundaryContainer.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        float half = boundarySize / 2f;

        // Create four edge colliders (top, bottom, left, right)
        CreateEdge("TopEdge", new Vector2(-half, half), new Vector2(half, half));
        CreateEdge("BottomEdge", new Vector2(-half, -half), new Vector2(half, -half));
        CreateEdge("LeftEdge", new Vector2(-half, -half), new Vector2(-half, half));
        CreateEdge("RightEdge", new Vector2(half, -half), new Vector2(half, half));

        // Create visible boundaries
        CreateVisualBoundary();
        PhysicsMaterial2D noFrictionMaterial = new PhysicsMaterial2D();
        noFrictionMaterial.friction = 0f;
        noFrictionMaterial.bounciness = 0f;

        // Apply to all edge colliders
        foreach (EdgeCollider2D edge in boundaryContainer.GetComponentsInChildren<EdgeCollider2D>())
        {
            edge.sharedMaterial = noFrictionMaterial;
        }
    }

    void CreateEdge(string name, Vector2 start, Vector2 end)
    {
        GameObject edge = new GameObject(name);
        edge.transform.parent = boundaryContainer.transform;
        edge.layer = boundaryLayer;

        EdgeCollider2D edgeCollider = edge.AddComponent<EdgeCollider2D>();
        edgeCollider.points = new Vector2[] { start, end };
    }

    void CreateVisualBoundary()
    {
        visualLinesContainer = new GameObject("BoundaryVisuals");
        visualLinesContainer.transform.position = player.transform.position;

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
        // Create a properly sized white sprite
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
}
