using UnityEngine;

public class PlayerSwim : MonoBehaviour
{
    [Header("당신의 호흡이 딸리는 수치")]
    private PlayerMovement _playerMovement;
    private Rigidbody2D _rb;
    private Animator _animator;
    public WaterUI _WaterUI;

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
