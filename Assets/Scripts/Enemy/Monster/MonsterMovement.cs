using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    private float _direction = 1f; // 1 = 오른쪽, -1 = 왼쪽


    private float moveSpeed = 3f;
    public float jumpForce = 8f;

    // LayerMask로 선언해야 함!!!
    public LayerMask wallLayer;

    private Rigidbody2D _rb;
    [SerializeField] private bool _isFacingRight = true;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        // 만약 인스펙터에서 지정하지 않았다면 자동 설정
        if (wallLayer == 0)
            wallLayer = 1 << LayerMask.NameToLayer("Wall");
    }

    public void Move(float dir)
    {
        _rb.velocity = new Vector2(_direction * dir * moveSpeed, _rb.velocity.y);

        if (IsWallAhead())
            Flip();

    }

    public bool IsWallAhead()
    {
        float dir = _isFacingRight ? 1f : -1f;

        Vector2 origin = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            Vector2.right * dir,
            0.6f,
            wallLayer
        );

        return hit.collider != null;
    }

    public void Flip()
    {
        _direction *= -1;  // **방향 반전**
        _isFacingRight = !_isFacingRight;
        Vector3 s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
    }
}