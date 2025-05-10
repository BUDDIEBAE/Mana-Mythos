using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomPatrolNPC4 : MonoBehaviour
{
    public float walkRadius = 10f;
    public float waitTime = 2f;
    public float moveSpeed = 3.5f;

    private NavMeshAgent agent;
    private float timer;
    public Animator animator;
    public float currentZ = 0;
    public float currentX = 0;

    private Camera mainCamera;
    private Transform spriteTransform;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        timer = waitTime;
        SetNewRandomDestination();

        animator = GetComponent<Animator>();
        mainCamera = Camera.main;

        // หาลูก GameObject ที่ชื่อว่า "Sprite" เพื่อใช้หมุนเข้าหากล้อง
        spriteTransform = transform.Find("Sprite");
        if (spriteTransform == null) spriteTransform = transform; // fallback
    }

    void Update()
    {
        FaceCamera2D();

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

        animator.SetBool("isMoving", agent.velocity.magnitude > 0.1f);
    }

    void SetNewRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, walkRadius, NavMesh.AllAreas))
        {
            Vector3 target = hit.position;

            if (currentZ > target.z)
                animator.SetInteger("DIR3", 0); // ขึ้น
            else if (currentZ < target.z)
                animator.SetInteger("DIR3", 1); // ลง

            if (currentX > target.x)
                animator.SetInteger("DIR3", 2); // ซ้าย
            else if (currentX < target.x)
                animator.SetInteger("DIR3", 3); // ขวา

            agent.SetDestination(target);
        }
    }

    void FaceCamera2D()
    {
        Vector3 camPosition = mainCamera.transform.position;
        Vector3 lookDir = camPosition - spriteTransform.position;
        lookDir.y = 0;

        if (lookDir.sqrMagnitude > 0.001f)
        {
            Quaternion rotation = Quaternion.LookRotation(-lookDir);
            spriteTransform.rotation = rotation;
        }
    }
}
