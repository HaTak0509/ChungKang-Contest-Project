using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float walkSpeed;

    private Vector2 _moveInput;
    private Rigidbody _rb;

    public float CurrentMoveSpeed
    {
        get
        {
            if (!isMoving) return 0;
            return walkSpeed;
        }
    }

    private bool _IsMoving = false;
    public bool isMoving
    {
        get { return _IsMoving; }
        set
        {
            _IsMoving = value;
        }
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        _rb.velocity = new Vector2(_moveInput.x * CurrentMoveSpeed, _rb.velocity.y);
    }

    public void OnMoveInputAction(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();

        isMoving = _moveInput != Vector2.zero;
    }
}
