using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int gridWidth = 8;
    [SerializeField] private int gridHeight = 8;
    [SerializeField] private float cellSize = 1.0f;
    
    [Header("Prefabs")]
    [SerializeField] private GameObject[] itemPrefabs; // Arrastra tus 3 prefabs aquí
    
    // Grid data
    private GameObject[,] gridItems;
    private Vector2 gridOrigin = new Vector2(-4f, -4f);
    
    public static GridManager Instance;
    
    void Awake()
    {
        Instance = this;
        gridItems = new GameObject[gridWidth, gridHeight];
    }
    
    void Start()
    {
        CreateVisualGrid();
        SpawnInitialItems();
    }
    
    // Crea el grid visual
    void CreateVisualGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 pos = GetWorldPosition(x, y);
                GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Quad);
                cell.name = $"Cell_{x}_{y}";
                cell.transform.position = pos;
                cell.transform.localScale = new Vector3(cellSize * 0.9f, cellSize * 0.9f, 1);
                
                // Color gris claro
                cell.GetComponent<Renderer>().material.color = new Color(0.8f, 0.8f, 0.8f, 0.5f);
                cell.transform.parent = transform;
                
                // Quitar collider
                Destroy(cell.GetComponent<Collider>());
            }
        }
    }
    
    // Spawn items iniciales para probar
    void SpawnInitialItems()
    {
        // 5 items aleatorios
        for (int i = 0; i < 5; i++)
        {
            int randX = Random.Range(0, gridWidth);
            int randY = Random.Range(0, gridHeight);
            
            if (gridItems[randX, randY] == null)
            {
                SpawnItem(0, randX, randY); // tier 0 (rojo)
            }
        }
    }
    
    // Spawn un item en posición específica
    public void SpawnItem(int tier, int x, int y)
    {
        if (tier >= itemPrefabs.Length) return;
        if (gridItems[x, y] != null) return;
        
        Vector3 pos = GetWorldPosition(x, y);
        GameObject item = Instantiate(itemPrefabs[tier], pos, Quaternion.identity);
        item.transform.parent = transform;
        
        gridItems[x, y] = item;
    }
    
    // Convierte coordenadas de grid a mundo
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(
            gridOrigin.x + (x * cellSize),
            gridOrigin.y + (y * cellSize),
            0
        );
    }
    
    // Convierte posición del mundo a grid
    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt((worldPos.x - gridOrigin.x) / cellSize);
        int y = Mathf.RoundToInt((worldPos.y - gridOrigin.y) / cellSize);
        
        x = Mathf.Clamp(x, 0, gridWidth - 1);
        y = Mathf.Clamp(y, 0, gridHeight - 1);
        
        return new Vector2Int(x, y);
    }
    
    // Verifica si celda está vacía
    public bool IsCellEmpty(int x, int y)
    {
        if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight) return false;
        return gridItems[x, y] == null;
    }
    
    // Debug: dibuja grid en Scene view
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 pos = new Vector3(
                    gridOrigin.x + (x * cellSize),
                    gridOrigin.y + (y * cellSize),
                    0
                );
                Gizmos.DrawWireCube(pos, Vector3.one * cellSize);
            }
        }
    }
}