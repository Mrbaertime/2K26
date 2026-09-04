using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootEvery = 2f;
    [SerializeField] private float arrowSpeed = 18f;

    void Start()
    {
        InvokeRepeating(nameof(Shoot), 1f, shootEvery);
    }

    void Shoot()
    {
        GameObject arrow = Instantiate(
            arrowPrefab,
            firePoint.position,
            firePoint.rotation
        );

        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.linearVelocity = firePoint.forward * arrowSpeed;
    }
}