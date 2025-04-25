using UnityEngine;

public class AlgoritmoCelular : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int width = 160;
    [SerializeField] private int height = 90;
    [SerializeField][Range(0, 100)] private int noiseDensity = 50;
    [SerializeField] private int iterations = 5;

    [Header("Tiles")]
    [SerializeField] private TileBase whiteTile;
    [SerializeField] private TileBase blackTile;

    private Tilemap tilemap;
    private int[,] grid;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        InitializeGrid();
        GenerateProceduralMap();
    }

    // Inicializa el grid con todas las celdas en negro (por defecto)
    private void InitializeGrid()
    {
        grid = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = 0; // 0 = negro, 1 = blanco
            }
        }
    }

    // Genera el mapa procedural
    public void GenerateProceduralMap()
    {
        GenerateNoise();
        for (int i = 0; i < iterations; i++)
        {
            ApplyCellularAutomataRules();
        }
        DrawTilemap();
    }

    // Genera ruido inicial
    private void GenerateNoise()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = (Random.Range(0, 100) > noiseDensity) ? 1 : 0;
            }
        }
    }

    // Aplica las reglas del autómata celular
    private void ApplyCellularAutomataRules()
    {
        int[,] newGrid = (int[,])grid.Clone();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighborWallCount = CountNeighborWalls(x, y);
                bool isBorder = (x == 0 || y == 0 || x == width - 1 || y == height - 1);

                // Regla: Si tiene más de 4 vecinos negros o es borde, se convierte en negro
                newGrid[x, y] = (neighborWallCount > 4 || isBorder) ? 0 : 1;
            }
        }

        grid = newGrid;
    }

    // Cuenta los vecinos negros (incluyendo diagonales)
    private int CountNeighborWalls(int x, int y)
    {
        int count = 0;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue; // Ignora la celda actual

                int neighborX = x + i;
                int neighborY = y + j;

                if (neighborX >= 0 && neighborX < width && neighborY >= 0 && neighborY < height)
                {
                    if (grid[neighborX, neighborY] == 0) count++;
                }
            }
        }
        return count;
    }

    // Dibuja el tilemap
    private void DrawTilemap()
    {
        tilemap.ClearAllTiles();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                TileBase tile = (grid[x, y] == 1) ? whiteTile : blackTile;
                tilemap.SetTile(tilePosition, tile);
            }
        }
    }

    // Métodos para UI/Inputs (ejemplo con sliders)
    public void SetNoiseDensity(float density) => noiseDensity = Mathf.RoundToInt(density);
    public void SetIterations(int iterations) => this.iterations = iterations;
    public void RegenerateMap() => GenerateProceduralMap();
}
