using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] float jumpForce = 14f;

    private Rigidbody2D _rb2D;
    private TouchingDetection _ground;
    private Damageable _damageable;

    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _ground = GetComponent<TouchingDetection>();
        _damageable = GetComponent<Damageable>();
    }

    public void TryJump()
    {
        if (!_ground.IsGround) return;
        if (_damageable.IsKnockback) return;

        _rb2D.velocity = new Vector2(_rb2D.velocity.x, jumpForce);
    }
}