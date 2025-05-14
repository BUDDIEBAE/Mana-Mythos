using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RandomPatrol : MonoBehaviour
{
    public float walkRadius = 10f;
    public float waitTime = 2f;
    public float moveSpeed = 3.5f;

    private NavMeshAgent agent;
    private float timer;
    private Animator animator;
    private Vector3 lastPosition;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        timer = waitTime;

        animator = GetComponent<Animator>();
        lastPosition = transform.position;

        SetNewRandomDestination();
    }

    void Update()
    {
        UpdateAnimation();

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            timer += Time.deltaTime;
            if (timer >= waitTime)
            {
                SetNewRandomDestination();
                timer = 0f;
            }
        }
    }

    void SetNewRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection.y = 0;
        Vector3 destination = transform.position + randomDirection;

        if (NavMesh.SamplePosition(destination, out NavMeshHit hit, walkRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    void UpdateAnimation()
    {
        Vector3 velocity = (transform.position - lastPosition) / Time.deltaTime;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);

        animator?.SetFloat("MoveX", localVelocity.x);
        animator.SetFloat("MoveZ", localVelocity.z);

        lastPosition = transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Despawn"))
        {
            SpawnManager.Instance.SpawnNewNPC();
            Destroy(gameObject);
        }
    }
    void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward;
    }
}