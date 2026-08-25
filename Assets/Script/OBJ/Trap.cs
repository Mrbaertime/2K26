using UnityEngine;

public class Trap : MonoBehaviour
{
    // กรณีที่ 1: กับดักเป็นแบบ Trigger (ติ๊ก Is Trigger - เดินทะลุได้ เช่น พื้นที่อาบยาพิษ)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateTrap(other.gameObject);
        }
    }

    // กรณีที่ 2: กับดักเป็นวัตถุแข็ง (ไม่ได้ติ๊ก Is Trigger - เช่น หนามโลหะ หรือกล่องกับดัก)
    private void OnCollisionEnter(Collision collision)
    {
        // สังเกตว่า Collision จะใช้ .gameObject นำหน้าก่อนเรียก CompareTag
        if (collision.gameObject.CompareTag("Player"))
        {
            ActivateTrap(collision.gameObject);
        }
    }

    // สร้างฟังก์ชันแยกออกมา เพื่อไม่ให้ต้องเขียนโค้ดซ้ำกัน
    private void ActivateTrap(GameObject target)
    {
        Debug.Log("โดนกับดัก! Game Over");

        // ทำลายผู้เล่น
        Destroy(target);
    }
}