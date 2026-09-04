using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private DoorController door;
    private readonly HashSet<Collider> playersOnPlate = new();

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersOnPlate.Add(other);
            door.SetOpen(gameObject.name, true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersOnPlate.Remove(other);

            if (playersOnPlate.Count == 0)
                door.SetOpen(gameObject.name, false);
        }
    }
}