using UnityEngine;

public class DestroyOnStart : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject); // ทำลายวัตถุนี้เมื่อเริ่มฉาก
    }
}
