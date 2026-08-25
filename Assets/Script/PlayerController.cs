using UnityEngine;
using UnityEngine.InputSystem;

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

        if (!Physics.Raycast(
            ray,
            out RaycastHit hit))
        {
            return;
        }

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