using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float SprintSpeed;
    [SerializeField] private float jumpImpulse;
    [SerializeField] private float dashImpulse;

    private Vector3 baseScale;
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

    private bool _isDash = false;

    public bool IsDash
    {
        get { return _isDash; }
        set
        {
            _isDash = value;
        }
    }

    void Start()
    {
        baseScale = transform.localScale;
        _rb = GetComponent<Rigidbody2D>();
        _touchingDetection = GetComponent<TouchingDetection>();
    }

    void FixedUpdate()
    {
        if (IsDash) return;

        _rb.velocity = new Vector2((_moveInput.x * CurrentMoveSpeed), _rb.velocity.y); // 속도 계산
    }

    public void OnMoveInputAction(InputAction.CallbackContext context) // 움직임 입력 계산
    {
        _moveInput = context.ReadValue<Vector2>();

        IsMoving = _moveInput != Vector2.zero;
        Facing(_moveInput);
    }

    public void OnJumpInputAction (InputAction.CallbackContext context) // 점프
    {
        if (context.started && _touchingDetection.IsGround) // 바닥에 닿아있다면 점프
        {
            _rb.velocity = new Vector2(_rb.velocity.x, jumpImpulse);
        }
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

    public void OnInteractionInputAction (InputAction.CallbackContext context) // 상호작용 입력 계산
    {

    }

    public void OnDashInputAction (InputAction.CallbackContext context) // 대쉬 입력 계산
    {
        if (IsDash) return; // 이미 대쉬가 실행중이라면 발동X

        if (context.started)
        {
            StartCoroutine(DashRoutine()); // 코루틴으로 대쉬 실행
        }
    }

    private IEnumerator DashRoutine() // 대쉬 거리 계산
    {
        IsDash = true ;

        float dir = _rb.transform.localScale.x > 0 ? 1 : -1; // Player가 보는 방향 구하기
        _rb.velocity = new Vector2(dashImpulse * dir, _rb.velocity.y); // 구한 방향에 따라 대쉬

        yield return new WaitForSeconds(0.15f); // 몇 초 안에 대쉬 거리를 이동할지
        
        IsDash = false ;
    }

    private void Facing(Vector2 input) // Player회전
    {
        float currentScaleY = transform.localScale.y;
        float currentScaleZ = transform.localScale.z;
        float currentScalex = transform.localScale.x;

        if (input.x < 0)
            transform.localScale = new Vector3(-currentScalex, currentScaleY, currentScaleZ);
        else
            transform.localScale = new Vector3(currentScalex, currentScaleY, currentScaleZ);
    }
}
