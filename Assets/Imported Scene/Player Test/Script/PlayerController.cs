using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] float _speed = 10f;

    [Header("Component References")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Animator _anim;


    private Vector2 _lastMoveDir;
    private Vector2 _movement;
    void Update()
    {
        OnMove();
    }

    private void OnMove()
    {
        if (_rb == null) return;

        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");

        OnAnimChange();

        _rb.MovePosition((Vector2)_rb.position + _movement.normalized * _speed * Time.fixedDeltaTime);
    }

    private void OnAnimChange()
    {
        _anim.SetFloat("MoveX", _lastMoveDir.x);
        _anim.SetFloat("MoveY", _lastMoveDir.y);
        _anim.SetBool("IsMoving", _movement != Vector2.zero);
    }

    // Interaction with other Objects


}
