using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float SprintSpeed;

    private Vector2 _moveInput;
    private Rigidbody2D _rb;
    private TouchingDetection _touchingDetection;

    public float CurrentMoveSpeed // 현재 속도를 계산
    {
        get
        {
            if (!IsMoving) return 0;// 걷지 않고 있다면 현재 이동 속도를 0으로 준다.
            if (IsSprint) return SprintSpeed;
            return walkSpeed;
        }
    }

    private bool _isMoving = false; // 걷고 있는지의 캡슐화 (set 안에 여러 내용이 담길 예정)
    public bool IsMoving
    {
        get { return _isMoving; }
        set
        {
            _isMoving = value;
        }
    }

    private bool _isSprint = false;

    public bool IsSprint // 달리고 있는지의 캡슐화 (set 안에 여러 내용이 담길 예정)
    {
        get { return _isSprint; }
        set
        {
            _isSprint = value;
        }
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        _rb.velocity = new Vector2((_moveInput.x * CurrentMoveSpeed), _rb.velocity.y); // 속도 계산
    }

    public void OnMoveInputAction(InputAction.CallbackContext context) // 움직임 입력 계산
    {
        _moveInput = context.ReadValue<Vector2>();

        IsMoving = _moveInput != Vector2.zero;
    }

    public void OnJumpInputAction (InputAction.CallbackContext context) // 점프
    {

    }

    public void OnSprintInputAction (InputAction.CallbackContext context) // 달리기 입력 계산
    {
        if (context.started) // 이벤트가 실행됬을 때 달리기
        {
            _isSprint = true;
        }

        if (context.canceled) // 이벤트 실행이 끝났을 때 달리기 멈추기
        {
            IsSprint = false;
        }
    }
}
