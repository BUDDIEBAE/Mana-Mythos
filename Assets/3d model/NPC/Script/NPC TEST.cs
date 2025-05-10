using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomPatrol : MonoBehaviour
{
    public float walkRadius = 10f;
    public float waitTime = 2f;
    public float moveSpeed = 3.5f;

    private NavMeshAgent agent;
    private float timer;
    public Animator animator;
    public float currentZ = 0;
    public float currentX = 0;

    private Camera mainCamera;         // <<< เพิ่มกล้องหลัก
    private Transform spriteTransform; // <<< สำหรับ sprite child

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        timer = waitTime;
        SetNewRandomDestination();

        animator = GetComponent<Animator>();
        mainCamera = Camera.main;

        // ถ้า Sprite อยู่ใน child ให้กำหนดชัดเจน
        spriteTransform = transform.Find("Sprite"); // เปลี่ยนชื่อ GameObject sprite ให้ตรงกับของคุณ
        if (spriteTransform == null) spriteTransform = transform; // fallback
    }

    void Update()
    {
        FaceCamera2D(); // <<< เรียกฟังก์ชันหมุนหากล้อง

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            timer += Time.deltaTime;
            if (timer >= waitTime)
            {
                currentZ = transform.position.z;
                currentX = transform.position.x;
                SetNewRandomDestination();
                timer = 0f;
            }
        }
    }

    void SetNewRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, walkRadius, NavMesh.AllAreas))
        {
            if (currentZ > hit.position.z)
            {
                animator.SetInteger("DIR", 0);
            }
            else if (currentZ < hit.position.z)
            {
                animator.SetInteger("DIR", 1);
            }

            if (currentX > hit.position.x)
            {
                animator.SetInteger("DIR", 2);
            }
            else if (currentX < hit.position.x)
            {
                animator.SetInteger("DIR", 3);
            }

            agent.SetDestination(hit.position);
        }
    }

    void FaceCamera2D()
    {
        // หัน sprite child ให้มองกล้อง
        Vector3 camPosition = mainCamera.transform.position;
        Vector3 lookDir = camPosition - spriteTransform.position;
        lookDir.y = 0; // ล็อกหมุนแนวแกน Y เท่านั้น

        if (lookDir.sqrMagnitude > 0.001f)
        {
            Quaternion rotation = Quaternion.LookRotation(-lookDir); // sprite หันด้านหลัง
            spriteTransform.rotation = rotation;
        }
    }
}
