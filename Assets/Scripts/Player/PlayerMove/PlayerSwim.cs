using UnityEngine;

public class PlayerSwim : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private Rigidbody2D _rb;
    private Animator _animator;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            _rb.gravityScale = 0;
            _playerMovement.SetSwimming(true);
            _animator.SetBool(AnimationStrings.IsSwim, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            _rb.gravityScale = 1;
            _playerMovement.SetSwimming(false);
            _animator.SetBool(AnimationStrings.IsSwim, false);
            _animator.SetBool(AnimationStrings.IsVerticalSwim, false);
            _animator.SetBool(AnimationStrings.IsHorizontalSwim, false);
        }
    }

}
