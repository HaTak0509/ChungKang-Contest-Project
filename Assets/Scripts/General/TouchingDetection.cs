using UnityEngine;

public class TouchingDetection : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private float groundCheckDistance = 0.1f;   // 바닥 체크 추가 거리
    [SerializeField] private float wallCheckDistance = 0.2f;     // 벽 체크 추가 거리

    [SerializeField] private bool showDebugRays = true;

    private CapsuleCollider2D _capsuleCollider;
    private PlayerMovement _playerMovement;
    private Animator _animator;

    // Public 프로퍼티들
    public bool IsGround { get; private set; }
    public bool IsOnWall { get; private set; }
    public int WallDirection { get; private set; }  // -1: 왼쪽 벽, 1: 오른쪽 벽, 0: 없음

    void Awake()
    {
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _animator = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    void FixedUpdate()
    {
        if (_playerMovement != null && _playerMovement.IsSwimming) return;
        
        CheckGround();
        CheckWalls();

        if (IsGround)
        {
            _animator.SetBool(AnimationStrings.IsGround, true);
        }
        else
        {
            _animator.SetBool(AnimationStrings.IsGround, false);
        }
    }

    private void CheckGround()
    {
        Bounds bounds = _capsuleCollider.bounds;
        Vector2 center = bounds.center;
        float halfHeight = bounds.extents.y;

        // 아래쪽 3지점에서 Raycast (중앙 + 왼쪽 + 오른쪽)
        Vector2 leftOrigin = new Vector2(center.x - bounds.extents.x + 0.1f, center.y);
        Vector2 centerOrigin = center;
        Vector2 rightOrigin = new Vector2(center.x + bounds.extents.x - 0.1f, center.y);

        float rayDistance = halfHeight + groundCheckDistance;

        bool leftHit = Physics2D.Raycast(leftOrigin, Vector2.down, rayDistance, groundLayer);
        bool centerHit = Physics2D.Raycast(centerOrigin, Vector2.down, rayDistance, groundLayer);
        bool rightHit = Physics2D.Raycast(rightOrigin, Vector2.down, rayDistance, groundLayer);

        IsGround = centerHit && (leftHit || rightHit);
    }

    private void CheckWalls()
    {
        Bounds bounds = _capsuleCollider.bounds;

        Vector2 size = bounds.size;
        Vector2 center = bounds.center;

        float castDistance = wallCheckDistance;

        RaycastHit2D leftHit = Physics2D.CapsuleCast(
            center,
            size,
            _capsuleCollider.direction,
            0f,
            Vector2.left,
            castDistance,
            groundLayer
        );

        RaycastHit2D rightHit = Physics2D.CapsuleCast(
            center,
            size,
            _capsuleCollider.direction,
            0f,
            Vector2.right,
            castDistance,
            groundLayer
        );

        IsOnWall = leftHit.collider != null || rightHit.collider != null;

        if (leftHit.collider != null && rightHit.collider == null)
            WallDirection = -1;
        else if (rightHit.collider != null && leftHit.collider == null)
            WallDirection = 1;
        else
            WallDirection = 0;
    }
}