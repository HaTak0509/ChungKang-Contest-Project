using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] float jumpForce = 2f;

    private Rigidbody2D _rb2D;
    private TouchingDetection _touchingDetection;
    private Damageable _damageable;
    private Animator _animator;

    private bool _isJump;
    private bool _notJumpSky;

    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _touchingDetection = GetComponent<TouchingDetection>();
        _damageable = GetComponent<Damageable>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (_touchingDetection.IsGround)
        {
            _isJump = false;
        }
        else if (!_touchingDetection.IsGround && !_isJump)
        {
            _notJumpSky = true;
            _animator.SetFloat(AnimationStrings.IsSky, _rb2D.velocity.y);
        }
        else if (!_touchingDetection.IsGround && _isJump)
        {
            _notJumpSky = false;
            _isJump = true;
        }
    }

    public void TryJump()
    {
        if (!_touchingDetection.IsGround || _damageable.IsKnockback) return;

        _isJump = true;

        _rb2D.velocity = new Vector2(_rb2D.velocity.x, jumpForce);

        _animator.SetTrigger(AnimationStrings.IsJump);

        SoundManager.Instance.PlaySFX("player_jump", SoundManager.SoundOutput.SFX, 1);
    }
}