using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private PlayerMove selectedPlayer;
    private Camera mainCamera;

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
        if (Mouse.current == null)
            return;

        if (!Mouse.current.leftButton.wasPressedThisFrame)
            return;

        Vector2 mousePosition =
            Mouse.current.position.ReadValue();

        Ray ray =
            mainCamera.ScreenPointToRay(mousePosition);

        // ตรวจทุก Collider ที่ Ray ชน
        RaycastHit[] hits =
            Physics.RaycastAll(ray);

        // เรียงจากสิ่งที่อยู่ใกล้ Camera → ไกล Camera
        System.Array.Sort(
            hits,
            (a, b) =>
                a.distance.CompareTo(b.distance)
        );

        foreach (RaycastHit hit in hits)
        {
            // =========================
            // คลิก Player
            // =========================

            PlayerMove clickedPlayer =
                hit.collider.GetComponent<PlayerMove>();

            if (clickedPlayer != null)
            {
                SelectPlayer(clickedPlayer);
                return;
            }

            // =========================
            // คลิก Tile
            // =========================

            Tile clickedTile =
                hit.collider.GetComponent<Tile>();

            if (clickedTile != null)
            {
                if (!clickedTile.isWalkable)
                    return;

                if (selectedPlayer != null)
                {
                    selectedPlayer.MoveToTile(
                        clickedTile
                    );
                }

                return;
            }

            // ถ้าเป็น Wall / Object อื่น
            // ให้ข้ามไปดู Collider ถัดไป
        }
    }

    private void SelectPlayer(
        PlayerMove player)
    {
        selectedPlayer = player;

        Debug.Log(
            "Selected: " +
            player.name
        );
    }
}