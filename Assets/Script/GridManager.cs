using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    private Dictionary<Vector2Int, Tile> tiles =
        new Dictionary<Vector2Int, Tile>();

    private void Awake()
    {
        Instance = this;
        RegisterAllTiles();
    }

    private void RegisterAllTiles()
    {
        Tile[] allTiles = FindObjectsByType<Tile>(
            FindObjectsSortMode.None
        );

        foreach (Tile tile in allTiles)
        {
            tile.UpdateGridPosition();

            if (tiles.ContainsKey(tile.gridPosition))
            {
                Debug.LogWarning(
                    "มี Tile ซ้อนกันที่: " +
                    tile.gridPosition
                );

                continue;
            }

            tiles.Add(
                tile.gridPosition,
                tile
            );
        }

        Debug.Log(
            "Registered Tiles: " +
            tiles.Count
        );
    }

    public Tile GetTile(Vector2Int position)
    {
        tiles.TryGetValue(
            position,
            out Tile tile
        );

        return tile;
    }

    public List<Tile> GetNeighbours(Tile tile)
    {
        List<Tile> neighbours =
            new List<Tile>();

        Vector2Int position =
            tile.gridPosition;

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
                position + direction;

            Tile neighbour =
                GetTile(neighbourPosition);

            if (neighbour != null &&
                neighbour.isWalkable)
            {
                neighbours.Add(neighbour);
            }
        }

        return neighbours;
    }
}