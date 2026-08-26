using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [Header("Grid Settings")]
    [SerializeField] private float tileSize = 1f;

    private Dictionary<Vector2Int, Tile> tiles =
        new Dictionary<Vector2Int, Tile>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        RegisterAllTiles();
    }

    private void RegisterAllTiles()
    {
        Tile[] allTiles =
            FindObjectsByType<Tile>(
                FindObjectsSortMode.None
            );

        foreach (Tile tile in allTiles)
        {
            Vector2Int gridPos = WorldToGrid(
                tile.transform.position
            );

            tile.gridPosition = gridPos;

            if (!tiles.ContainsKey(gridPos))
            {
                tiles.Add(gridPos, tile);
            }
            else
            {
                Debug.LogWarning(
                    "มี Tile ซ้อนกันที่ Grid Position: " +
                    gridPos
                );
            }
        }

        Debug.Log(
            "Registered Tiles: " + tiles.Count
        );
    }

    public Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(
            worldPosition.x / tileSize
        );

        int z = Mathf.RoundToInt(
            worldPosition.z / tileSize
        );

        return new Vector2Int(x, z);
    }

    public Tile GetTile(Vector2Int position)
    {
        if (tiles.TryGetValue(
            position,
            out Tile tile))
        {
            return tile;
        }

        return null;
    }

    public List<Tile> GetNeighbours(Tile tile)
    {
        List<Tile> neighbours =
            new List<Tile>();

        Vector2Int[] directions =
        {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighbourPosition =
                tile.gridPosition + direction;

            Tile neighbour =
                GetTile(neighbourPosition);

            if (neighbour != null &&
                neighbour.isWalkable &&
                !neighbour.IsOccupied)
            {
                neighbours.Add(neighbour);
            }
        }

        return neighbours;
    }
}