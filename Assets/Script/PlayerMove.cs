using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;

    [Header("Tile Detection")]
    [SerializeField] private float tileCheckDistance = 2f;

    private List<Tile> currentPath;
    private int pathIndex;
    private bool isMoving;

    public void MoveToTile(Tile targetTile)
    {
        if (isMoving)
            return;

        Tile currentTile = GetCurrentTile();

        if (currentTile == null)
        {
            Debug.LogError(
                name + " ไม่พบ Tile ที่กำลังยืนอยู่"
            );

            return;
        }

        if (Pathfinder.Instance == null)
        {
            Debug.LogError("ไม่พบ Pathfinder");
            return;
        }

        currentPath =
            Pathfinder.Instance.FindPath(
                currentTile,
                targetTile
            );

        if (currentPath == null)
        {
            Debug.Log("ไม่มีเส้นทางไปยัง Tile นี้");
            return;
        }

        pathIndex = 0;
        isMoving = currentPath.Count > 0;
    }

    private void Update()
    {
        MoveAlongPath();
    }

    private void MoveAlongPath()
    {
        if (!isMoving)
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

            pathIndex++;
        }
    }

    private Tile GetCurrentTile()
    {
        Ray ray = new Ray(
            transform.position + Vector3.up * 0.5f,
            Vector3.down
        );

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            tileCheckDistance
        ))
        {
            return hit.collider.GetComponent<Tile>();
        }

        return null;
    }

    public void SetSelected(bool selected)
    {
        // ตรงนี้ใช้กับระบบแสงที่เราทำไว้
    }
}