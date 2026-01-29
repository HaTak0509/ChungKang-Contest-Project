using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkSpeed = 6f;
    [SerializeField] float pushingSpeed = 4f;

    public bool moveLimit;

    private Rigidbody2D _rb2D;
    private Damageable _damageable;
    private PlayerFacing _facing;
    private TouchingDetection _touchingDetection;
    private Pushing _pushing;
    public Animator _animator;

    private Vector2 _moveInput;

    private float CurrentSpeed =>
        (_pushing.isPushing) ? pushingSpeed : walkSpeed;

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

    private void FixedUpdate()
    {
        if (_damageable != null && _damageable.IsInvincible)
            return;

        if (_pushing.isPushing)
        {
            _animator.SetBool(AnimationStrings.IsPushing, true);
        }
        else
        {
            _animator.SetBool(AnimationStrings.IsPushing, false);
        }

        float desiredX = 0f;

        if (!moveLimit)
        {
            desiredX = _moveInput.x * CurrentSpeed;

            // 벽 방향 제한
            if (_touchingDetection.IsOnWall)
            {
                if (_touchingDetection.WallDirection == -1 && desiredX < 0)
                    desiredX = 0;
                else if (_touchingDetection.WallDirection == 1 && desiredX > 0)
                    desiredX = 0;
            }
        }

        ApplyHorizontalVelocity(desiredX);
    }

    private void ApplyHorizontalVelocity(float desiredX)
    {
        // 핵심: Y는 절대 건드리지 않는다
        _rb2D.velocity = new Vector2(desiredX, _rb2D.velocity.y);
    }
}
