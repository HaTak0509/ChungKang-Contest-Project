using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float pushingSpeed = 4f;
    [SerializeField] private float swimSpeed = 4f;
    [SerializeField] private Vector2 colliderXSize = new Vector2(0.85f, 0.55f);
    [SerializeField] private Vector2 colliderYSize = new Vector2(0.55f, 0.85f);

    public Vector2 moveInput;
    
    private Rigidbody2D _rb2D;
    private Damageable _damageable;
    private PlayerFacing _facing;
    private TouchingDetection _touchingDetection;
    private SpriteRenderer _spriteRenderer;
    private CapsuleCollider2D _collider;
    private Pushing _pushing;
    private Animator _animator;
    private PlayerController _playerController;

    public bool IsSwimming { get; private set; }

    private float CurrentSpeed =>
        (_pushing.pushing) ? pushingSpeed : walkSpeed;

    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _damageable = GetComponent<Damageable>();
        _facing = GetComponent<PlayerFacing>();
        _touchingDetection = GetComponent<TouchingDetection>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CapsuleCollider2D>();
        _pushing = GetComponent<Pushing>();
        _animator = GetComponent<Animator>();
        _playerController = GetComponent<PlayerController>();
    }

    public void SetInput(Vector2 input)
    {
        moveInput = input;

        if (input.sqrMagnitude > 0.01f)
            _facing.FaceDirection(input.x);
    }

    public void SetSwimming(bool value)
    {
        if (IsSwimming == value)
            return;

        IsSwimming = value;

        if (!IsSwimming) EnterGround();
    }

    private void Update()
    {
        _animator.SetFloat(AnimationStrings.Land, _rb2D.velocity.y);

        if (!IsSwimming)
        {
            _spriteRenderer.flipY = false;
            return;
        }

        float swimY = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            swimY = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            swimY = -1f;
        }

        moveInput.y = swimY;
    }

    private void FixedUpdate()
    {
        if (_playerController.allLimit || _playerController.moveLimit)
        {
            moveInput = new Vector2(0f, _rb2D.velocity.y);
            _rb2D.velocity = new Vector2(0f, _rb2D.velocity.y);
            return;
        }

        if (_damageable != null && _damageable.IsInvincible)
            return;
        

        _animator.SetBool(AnimationStrings.IsPushing, _pushing.pushing);

        if (IsSwimming) SwimmingMovement();
        else GroundMovement();
    }

    private void GroundMovement()
    {
        _collider.direction = CapsuleDirection2D.Vertical;
        _collider.size = colliderYSize;

        float desiredX = 0f;

        desiredX = moveInput.x * CurrentSpeed;

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
        Vector2 swimVelocity = Vector2.zero;

        bool moveHorizontal = Mathf.Abs(moveInput.x) > 0.01f;
        bool moveVertical = Mathf.Abs(moveInput.y) > 0.01f;

        if (!moveHorizontal && moveVertical) // 수직
        {
            _animator.SetBool(AnimationStrings.IsVerticalSwim, true);
            _animator.SetBool(AnimationStrings.IsHorizontalSwim, false);

            if (moveInput.y > 0f)
            {
                _spriteRenderer.flipY = false;
            }
            else if (moveInput.y < 0f)
            {
                _spriteRenderer.flipY = true;
            }

            swimVelocity = new Vector2(0f, moveInput.y * swimSpeed);

            _collider.direction = CapsuleDirection2D.Vertical;
            _collider.size = colliderYSize;
        }
        else if (moveHorizontal && !moveVertical) // 수평
        {
            _animator.SetBool(AnimationStrings.IsVerticalSwim, false);
            _animator.SetBool(AnimationStrings.IsHorizontalSwim, true);

            _spriteRenderer.flipY = false;

            swimVelocity = new Vector2(moveInput.x * swimSpeed, 0f);
            _collider.direction = CapsuleDirection2D.Horizontal;
            _collider.size = colliderXSize;
        }
        else if (moveHorizontal && moveVertical) // 대각선
        {
            _animator.SetBool(AnimationStrings.IsVerticalSwim, false);
            _animator.SetBool(AnimationStrings.IsHorizontalSwim, true);

            _spriteRenderer.flipY = false;

            swimVelocity = moveInput.normalized * swimSpeed;
            _collider.direction = CapsuleDirection2D.Horizontal;
            _collider.size = colliderXSize;
        }

            _rb2D.velocity = swimVelocity;
    }

    private void ApplyHorizontalVelocity(float desiredX)
    {
        // 핵심: Y는 절대 건드리지 않는다
        _rb2D.velocity = new Vector2(desiredX, _rb2D.velocity.y);
    }

    private void EnterGround()
    {
        _collider.direction = CapsuleDirection2D.Vertical;
        _collider.size = colliderYSize;
    }

    public void StopMove()
    {
        _rb2D.velocity = new Vector2(0,0);
    }

    public void WalkSound()
    {
        SoundManager.Instance.PlaySFX("player_move", SoundManager.SoundOutput.SFX, 1, Random.Range(0.9f,1.1f));
    }

    public void SwimSound()
    {
        SoundManager.Instance.PlaySFX("player_swim", SoundManager.SoundOutput.SFX, 1, Random.Range(0.9f, 1.1f));

    }
    
}
