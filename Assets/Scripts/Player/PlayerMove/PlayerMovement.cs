using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkSpeed = 6f;
    [SerializeField] float sprintSpeed = 10f;

    private Rigidbody2D _rb2D;
    private Damageable _damageable;
    private PlayerFacing _facing;
    private TouchingDetection _touchingDetection;
    private Vector2 _moveInput;
    private bool _isSprinting;

    private float CurrentSpeed
    {
        get
        {
            if (_touchingDetection.IsOnWall) return 0; 
            if (_isSprinting) return sprintSpeed;
            return walkSpeed;
        }
    }

    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _damageable = GetComponent<Damageable>();
        _facing = GetComponent<PlayerFacing>();
        _touchingDetection = GetComponent<TouchingDetection>();
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

        _rb2D.velocity = new Vector2(_moveInput.x * CurrentSpeed, _rb2D.velocity.y);
    }
}