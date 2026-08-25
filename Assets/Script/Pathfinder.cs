using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public static Pathfinder Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public List<Tile> FindPath(
        Tile startTile,
        Tile targetTile
    )
    {
        if (startTile == null || targetTile == null)
            return null;

        if (!targetTile.isWalkable)
            return null;

        List<Tile> openSet =
            new List<Tile>();

        HashSet<Tile> closedSet =
            new HashSet<Tile>();

        Dictionary<Tile, Tile> cameFrom =
            new Dictionary<Tile, Tile>();

        Dictionary<Tile, int> gScore =
            new Dictionary<Tile, int>();

        Dictionary<Tile, int> fScore =
            new Dictionary<Tile, int>();

        openSet.Add(startTile);

        gScore[startTile] = 0;

        fScore[startTile] =
            GetDistance(
                startTile,
                targetTile
            );

        while (openSet.Count > 0)
        {
            Tile currentTile =
                GetLowestFScore(
                    openSet,
                    fScore
                );

            if (currentTile == targetTile)
            {
                return RetracePath(
                    cameFrom,
                    currentTile
                );
            }

            openSet.Remove(currentTile);
            closedSet.Add(currentTile);

            foreach (Tile neighbour in
                     GridManager.Instance.GetNeighbours(currentTile))
            {
                if (closedSet.Contains(neighbour))
                    continue;

                int tentativeGScore =
                    GetScore(gScore, currentTile)
                    + 1;

                if (!openSet.Contains(neighbour))
                {
                    openSet.Add(neighbour);
                }
                else if (tentativeGScore >=
                         GetScore(gScore, neighbour))
                {
                    continue;
                }

                cameFrom[neighbour] =
                    currentTile;

                gScore[neighbour] =
                    tentativeGScore;

                fScore[neighbour] =
                    tentativeGScore +
                    GetDistance(
                        neighbour,
                        targetTile
                    );
            }
        }

        // หาเส้นทางไม่เจอ
        return null;
    }

    private Tile GetLowestFScore(
        List<Tile> tiles,
        Dictionary<Tile, int> fScore
    )
    {
        Tile lowestTile = tiles[0];
        int lowestScore =
            GetScore(fScore, lowestTile);

        foreach (Tile tile in tiles)
        {
            int score =
                GetScore(fScore, tile);

            if (score < lowestScore)
            {
                lowestScore = score;
                lowestTile = tile;
            }
        }

        return lowestTile;
    }

    private int GetScore(
        Dictionary<Tile, int> scores,
        Tile tile
    )
    {
        if (scores.TryGetValue(
            tile,
            out int score
        ))
        {
            return score;
        }

        return int.MaxValue;
    }

    private List<Tile> RetracePath(
        Dictionary<Tile, Tile> cameFrom,
        Tile currentTile
    )
    {
        List<Tile> path =
            new List<Tile>();

        path.Add(currentTile);

        while (cameFrom.ContainsKey(currentTile))
        {
            currentTile =
                cameFrom[currentTile];

            path.Add(currentTile);
        }

        path.Reverse();

        // เอา Start Tile ออก
        if (path.Count > 0)
        {
            path.RemoveAt(0);
        }

        return path;
    }

    private int GetDistance(
        Tile a,
        Tile b
    )
    {
        int distanceX =
            Mathf.Abs(
                a.gridPosition.x -
                b.gridPosition.x
            );

        int distanceZ =
            Mathf.Abs(
                a.gridPosition.y -
                b.gridPosition.y
            );

        return distanceX + distanceZ;
    }
}