using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkSpeed = 6f;
    [SerializeField] float sprintSpeed = 10f;

    private Rigidbody2D _rb2D;
    private Damageable _damageable;
    private PlayerFacing _facing;
    private Vector2 _moveInput;
    private bool _isSprinting;

    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _damageable = GetComponent<Damageable>();
        _facing = GetComponent<PlayerFacing>();
    }

    public void SetInput(Vector2 input)
    {
        _moveInput = input;
        if (input.sqrMagnitude > 0.01f)
            _facing.FaceDirection(input.x);
    }

    public void SetSprinting(bool value) => _isSprinting = value;

    private void FixedUpdate()
    {
        if (_damageable != null && _damageable.IsInvincible)
        {
            return;
        }

        float speed = _isSprinting ? sprintSpeed : walkSpeed;

        _rb2D.velocity = new Vector2(_moveInput.x * speed, _rb2D.velocity.y);
    }
}