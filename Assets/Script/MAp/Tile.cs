using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Tile Settings")]
    public bool isWalkable = true;

    [Header("Grid Position")]
    public Vector2Int gridPosition;

    [Header("Occupant")]
    [SerializeField] private GameObject occupant;

    public bool IsOccupied
    {
        get
        {
            return occupant != null;
        }
    }

    public GameObject Occupant
    {
        get
        {
            return occupant;
        }
    }

    // ใส่คน/Enemy ลง Tile
    public bool SetOccupant(GameObject newOccupant)
    {
        if (IsOccupied && occupant != newOccupant)
        {
            return false;
        }

        occupant = newOccupant;

        return true;
    }

    // เอาคนออกจาก Tile
    public void ClearOccupant(GameObject target)
    {
        if (occupant == target)
        {
            occupant = null;
        }
    }
}