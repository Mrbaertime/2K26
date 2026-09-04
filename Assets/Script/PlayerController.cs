using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private PlayerMove selectedPlayer;
    private Camera mainCamera;

    // เก็บรายการช่องที่กำลังแสดงสีอยู่
    private List<Tile> currentHighlightedTiles = new List<Tile>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        HandleClick();
    }

    private void HandleClick()
    {
        if (Mouse.current == null) return;
        if (!Mouse.current.leftButton.wasPressedThisFrame) return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);

        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit hit in hits)
        {
            // คลิก Player
            PlayerMove clickedPlayer = hit.collider.GetComponent<PlayerMove>();
            if (clickedPlayer != null)
            {
                SelectPlayer(clickedPlayer);
                return;
            }

            // คลิก Tile
            Tile clickedTile = hit.collider.GetComponent<Tile>();
            if (clickedTile != null)
            {
                // ถ้าเป็นช่องที่เดินไม่ได้ ให้เมินไปเลย
                if (!clickedTile.isWalkable) return;

                if (selectedPlayer != null)
                {
                    // สั่งเดิน และปิดสีไฮไลต์ทั้งหมด
                    selectedPlayer.MoveToTile(clickedTile);
                    ClearHighlights();
                }
                return;
            }
        }
    }

    private void SelectPlayer(PlayerMove player)
    {
        ClearHighlights(); // ล้างสีเก่าทิ้งก่อน
        selectedPlayer = player;
        Debug.Log("Selected: " + player.name);

        // เปลี่ยนมาใช้ GridManager แปลงพิกัดตัวละครหาแผ่นพื้นแทนการยิง Raycast
        Vector2Int gridPos = GridManager.Instance.WorldToGrid(player.transform.position);
        Tile playerTile = GridManager.Instance.GetTile(gridPos);

        if (playerTile != null)
        {
            // ดึงเฉพาะช่องที่เดินเชื่อมถึงกันได้จริงๆ (ไม่ทะลุกำแพง)
            currentHighlightedTiles = Pathfinder.Instance.GetAllReachableTiles(playerTile);

            foreach (Tile tile in currentHighlightedTiles)
            {
                tile.ToggleHighlight(true);
            }
        }
        else
        {
            Debug.LogWarning("หาจุดที่ Player ยืนอยู่ไม่เจอ! เช็กพิกัด: " + gridPos);
        }
    }

    private void ClearHighlights()
    {
        foreach (Tile tile in currentHighlightedTiles)
        {
            tile.ToggleHighlight(false);
        }
        currentHighlightedTiles.Clear();
    }
}