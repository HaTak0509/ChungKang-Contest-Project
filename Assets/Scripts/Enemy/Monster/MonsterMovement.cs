using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float jumpForce = 8f;

    private Rigidbody2D _rb;
    private bool _isFacingRight = true;

    void Awake() { _rb = GetComponent<Rigidbody2D>(); }

    public void Move(float dir)
    {
        _rb.velocity = new Vector2(dir * moveSpeed, _rb.velocity.y);

        if ((dir > 0 && !_isFacingRight) || (dir < 0 && _isFacingRight))
            Flip();
    }

    public void Jump()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
    }

    public void Stop() => _rb.velocity = new Vector2(0, _rb.velocity.y);

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
    }
}