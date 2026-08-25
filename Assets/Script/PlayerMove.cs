using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;

    private List<Tile> currentPath;

    private int pathIndex;

    private Tile currentTile;

    private bool isMoving;

    private void Start()
    {
        FindStartingTile();
    }

    private void Update()
    {
        MoveAlongPath();
    }

    private void FindStartingTile()
    {
        if (GridManager.Instance == null)
        {
            Debug.LogError(
                "ไม่พบ GridManager!"
            );

            return;
        }

        Vector2Int playerGridPosition =
            GridManager.Instance.WorldToGrid(
                transform.position
            );

        currentTile =
            GridManager.Instance.GetTile(
                playerGridPosition
            );

        if (currentTile == null)
        {
            Debug.LogError(
                name +
                " ไม่ได้อยู่บน Tile!"
            );
        }
    }

    public void MoveToTile(Tile targetTile)
    {
        if (isMoving)
            return;

        if (currentTile == null)
        {
            FindStartingTile();
        }

        if (currentTile == null)
            return;

        currentPath =
            Pathfinder.Instance.FindPath(
                currentTile,
                targetTile
            );

        if (currentPath == null)
        {
            Debug.Log(
                "ไม่มีเส้นทางไปที่ " +
                targetTile.name
            );

            return;
        }

        pathIndex = 0;

        // ถ้า Path ตัวแรกคือ Tile ที่กำลังยืนอยู่
        if (currentPath.Count > 0 &&
            currentPath[0] == currentTile)
        {
            pathIndex = 1;
        }

        isMoving = true;
    }

    private void MoveAlongPath()
    {
        if (!isMoving)
            return;

        if (currentPath == null)
            return;

        if (pathIndex >= currentPath.Count)
        {
            isMoving = false;
            return;
        }

        Tile targetTile =
            currentPath[pathIndex];

        Vector3 targetPosition =
            targetTile.transform.position;

        targetPosition.y =
            transform.position.y;

        transform.position =
            Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

        if (Vector3.Distance(
            transform.position,
            targetPosition
        ) < 0.01f)
        {
            transform.position =
                targetPosition;

            currentTile =
                targetTile;

            pathIndex++;
        }
    }

    public Tile GetCurrentTile()
    {
        return currentTile;
    }
}