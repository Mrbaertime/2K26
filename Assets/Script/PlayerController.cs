using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [Header("Selection")]
    [SerializeField] private GameObject selectedIndicator;

    private PlayerMove selectedPlayer;
    private Camera mainCamera;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        mainCamera = Camera.main;

        // ตอนเริ่มเกมยังไม่เลือกใคร
        selectedPlayer = null;

        if (selectedIndicator != null)
        {
            selectedIndicator.SetActive(false);
        }
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

        if (!Physics.Raycast(ray, out RaycastHit hit))
            return;

        // คลิก Player
        PlayerMove clickedPlayer =
            hit.collider.GetComponent<PlayerMove>();

        if (clickedPlayer != null)
        {
            SelectPlayer(clickedPlayer);
            return;
        }

        // คลิก Tile
        Tile clickedTile =
            hit.collider.GetComponent<Tile>();

        if (clickedTile != null &&
            selectedPlayer != null)
        {
            selectedPlayer.MoveToTile(clickedTile);
        }
    }

    private void SelectPlayer(PlayerMove player)
    {
        selectedPlayer = player;

        Debug.Log(
            "Selected Player: " +
            player.name
        );

        // แสดงวงเลือก
        if (selectedIndicator != null)
        {
            selectedIndicator.SetActive(true);

            selectedIndicator.transform.SetParent(
                player.transform
            );

            selectedIndicator.transform.localPosition =
                Vector3.zero;
        }
    }

    public PlayerMove GetSelectedPlayer()
    {
        return selectedPlayer;
    }
}