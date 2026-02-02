using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkSpeed = 6f;
    [SerializeField] float pushingSpeed = 4f;
    [SerializeField] float swimSpeed = 4f;

    private Rigidbody2D _rb2D;
    private Damageable _damageable;
    private PlayerFacing _facing;
    private TouchingDetection _touchingDetection;
    private Pushing _pushing;
    private Animator _animator;

    private Vector2 _moveInput;

    public bool IsSwimming { get; private set; }

    private float CurrentSpeed =>
        (_pushing.pushing) ? pushingSpeed : walkSpeed;

    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _damageable = GetComponent<Damageable>();
        _facing = GetComponent<PlayerFacing>();
        _touchingDetection = GetComponent<TouchingDetection>();
        _pushing = GetComponent<Pushing>();
        _animator = GetComponent<Animator>();
    }

    public void SetInput(Vector2 input)
    {
        _moveInput = input;

        if (input.sqrMagnitude > 0.01f)
            _facing.FaceDirection(input.x);
    }

    public void SetSwimming(bool value)
    {
        IsSwimming = value;
    }

    private void FixedUpdate()
    {
        if (_damageable != null && _damageable.IsInvincible)
            return;

        _animator.SetBool(AnimationStrings.IsPushing, _pushing.pushing);

        if (IsSwimming) SwimmingMovement();
        else GroundMovement();
    }

    private void GroundMovement()
    {
        if (_pushing.isPushing && _moveInput.x != 0 && Mathf.Sign(_moveInput.x) != Mathf.Sign(_pushing.pushingDirection))
            return;

        float desiredX = 0f;

        desiredX = _moveInput.x * CurrentSpeed;

        if (_touchingDetection.IsOnWall)
        {
            if (_touchingDetection.WallDirection == -1 && desiredX < 0)
                desiredX = 0;
            else if (_touchingDetection.WallDirection == 1 && desiredX > 0)
                desiredX = 0;
        }

        ApplyHorizontalVelocity(desiredX);
    }

    private void SwimmingMovement()
    {
        Vector2 swimVelocity = _moveInput * swimSpeed;
        _rb2D.velocity = swimVelocity;
    }

    private void ApplyHorizontalVelocity(float desiredX)
    {
        // 핵심: Y는 절대 건드리지 않는다
        _rb2D.velocity = new Vector2(desiredX, _rb2D.velocity.y);
    }
}
