using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    public GameObject[] npcPrefabs; // รองรับหลายตัว
    public Transform spawnPoint;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SpawnNewNPC(); // ตัวแรกตอนเริ่ม
    }

    // Spawn แบบสุ่ม NPC ใด ๆ ในรายการ
    public void SpawnNewNPC()
    {
        int index = Random.Range(0, npcPrefabs.Length);
        SpawnNewNPC(index);
    }

    // Spawn NPC ตาม index ที่ระบุ
    public void SpawnNewNPC(int npcIndex)
    {
        if (npcIndex < 0 || npcIndex >= npcPrefabs.Length)
        {
            Debug.LogWarning("Invalid NPC index.");
            return;
        }

        Vector3 offset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        Instantiate(npcPrefabs[npcIndex], spawnPoint.position + offset, Quaternion.identity);
    }
}
