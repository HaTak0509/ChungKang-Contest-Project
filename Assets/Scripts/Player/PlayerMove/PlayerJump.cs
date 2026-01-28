using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] float jumpForce = 2f;

    private Rigidbody2D _rb2D;
    private TouchingDetection _ground;
    private Damageable _damageable;
    private Animator _animator;

    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _ground = GetComponent<TouchingDetection>();
        _damageable = GetComponent<Damageable>();
        _animator = GetComponent<Animator>();
    }

    public void TryJump()
    {
        if (!_ground.IsGround) return;
        if (_damageable.IsKnockback) return;

        _rb2D.velocity = new Vector2(_rb2D.velocity.x, jumpForce);
        _animator.SetTrigger(AnimationStrings.IsJump);
    }
}