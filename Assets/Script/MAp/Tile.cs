using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Tile Settings")]
    public bool isWalkable = true;

    public Vector2Int gridPosition;

    private void Awake()
    {
        UpdateGridPosition();
    }

    public void UpdateGridPosition()
    {
        gridPosition = new Vector2Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.z)
        );
    }
}
