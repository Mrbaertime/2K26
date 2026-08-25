using UnityEngine;
using UnityEngine.InputSystem; // 1. เพิ่มบรรทัดนี้เพื่อเรียกใช้ New Input System

public class WinBox : MonoBehaviour
{
    private bool isPlayerNear = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            Debug.Log("กด E เพื่อจบด่านนนนนนน");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }

    private void Update()
    {
        // 2. เช็คว่ามีคีย์บอร์ดเชื่อมต่ออยู่ และเช็คการกดปุ่ม E จากระบบใหม่
        if (isPlayerNear && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        Debug.Log("win");
        // โค้ดสำหรับจบเกม เช่น เรียกหน้าต่าง UI ชนะ 
    }
}