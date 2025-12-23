using UnityEngine;

public class TouchingDetection : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private float groundCheckDistance = 0.1f;   // 바닥 체크 추가 거리
    [SerializeField] private float wallCheckDistance = 0.2f;     // 벽 체크 추가 거리

    [SerializeField] private bool showDebugRays = true;

    private CapsuleCollider2D _capsuleCollider;

    // Public 프로퍼티들
    public bool IsGround { get; private set; }
    public bool IsOnWall { get; private set; }
    public int WallDirection { get; private set; }  // -1: 왼쪽 벽, 1: 오른쪽 벽, 0: 없음

    void Awake()
    {
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    void FixedUpdate()  // 물리 관련은 FixedUpdate가 더 안정적
    {
        CheckGround();
        CheckWalls();
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

        IsGround = leftHit || centerHit || rightHit;
    }

    private void CheckWalls()
    {
        Bounds bounds = _capsuleCollider.bounds;
        Vector2 center = bounds.center;
        float halfWidth = bounds.extents.x;

        float rayDistance = halfWidth + wallCheckDistance;

        // 왼쪽 벽 체크
        bool leftWallHit = Physics2D.Raycast(center, Vector2.left, rayDistance, groundLayer);

        // 오른쪽 벽 체크
        bool rightWallHit = Physics2D.Raycast(center, Vector2.right, rayDistance, groundLayer);

        // 벽에 닿아 있는지 여부
        IsOnWall = leftWallHit || rightWallHit;

        // 어느 쪽 벽인지 방향 반환 (-1: 왼쪽, 1: 오른쪽, 0: 없음)
        if (leftWallHit && !rightWallHit)
            WallDirection = -1;
        else if (rightWallHit && !leftWallHit)
            WallDirection = 1;
        else
            WallDirection = 0;  // 양쪽 다 닿아 있거나 없음
    }
}