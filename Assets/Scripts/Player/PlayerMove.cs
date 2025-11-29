using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float walkSpeed;

    private Vector2 _moveInput;
    private Rigidbody _rb;

    public float CurrentMoveSpeed // 현재 속도를 계산
    {
        get
        {
            if (!isMoving) return 0; // 걷지 않고 있다면 현재 이동 속도를 0으로 준다.
            return walkSpeed;
        }
    }

    private bool _IsMoving = false; // 걷고 있는지의 캡슐화 (set 안에 여러 내용이 담길 예정)
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
        _rb.velocity = new Vector2(_moveInput.x * CurrentMoveSpeed, _rb.velocity.y); // 속도 계산
    }

    public void OnMoveInputAction(InputAction.CallbackContext context) // 움직임 입력 계산
    {
        _moveInput = context.ReadValue<Vector2>();

        isMoving = _moveInput != Vector2.zero;
    }
}
