using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [SerializeField] float speed = 10f;
    [SerializeField] Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        OnMove();
    }

    void OnMove()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(x,0,z).normalized;

        rb.velocity = new Vector3(moveDir.x * speed, rb.velocity.y, moveDir.z * speed);


    }
}
