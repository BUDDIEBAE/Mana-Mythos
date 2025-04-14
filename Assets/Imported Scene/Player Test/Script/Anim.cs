using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim : MonoBehaviour
{

    [SerializeField] Animator anim;
    Movement movement;
    Rigidbody rb;

    void Awake()
    {
        anim= GetComponent<Animator>();
        movement= GetComponent<Movement>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        OnAnimChange();
    }
     enum animState 
    {
        front,
        back,
        left,
        right,
        idle
    }
    [SerializeField] int state;
    void OnAnimChange()
    {
        anim.SetInteger("State", state);
        if(rb.velocity.magnitude > 0f)
        {
            if(rb.velocity.x > 0f)
            {
                state = (int)animState.right;
            }
            else if (rb.velocity.x < 0f)
            {
                state = (int)animState.left;
            }
            else if (rb.velocity.z > 0f)
            {
                state= (int)animState.back;
            }
            else
            {
                state = (int)animState.front;
            }
        }
        else
        {
            state = (int)animState.idle;
        }

    }
}
