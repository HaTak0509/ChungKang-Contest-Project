using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkSpeed = 6f;
    [SerializeField] float pushingSpeed = 4; 

    public bool _isPushing;

    private Rigidbody2D _rb2D;
    private Damageable _damageable;
    private PlayerFacing _facing;
    private TouchingDetection _touchingDetection;
    private Vector2 _moveInput;

    private float CurrentSpeed
    {
        get
        {
            if (_isPushing) return pushingSpeed;
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

    private void FixedUpdate()
    {
        if (_damageable != null && _damageable.IsInvincible)
        {
            return;
        }

        // 왼쪽 벽에 닿은 상태에서 왼쪽을 보고 있다면
        if (_touchingDetection.WallDirection == -1 && transform.localScale.x < 0)
        {
            _rb2D.velocity = new Vector2(0, _rb2D.velocity.y);
        }
        else if (_touchingDetection.WallDirection == -1 && transform.localScale.x > 0)
        {
            _rb2D.velocity = new Vector2(_moveInput.x * CurrentSpeed, _rb2D.velocity.y);
        }
        
        // 오른쪽 벽에 닿은 상태에서 오른쪽을 보고 있다면
        if (_touchingDetection.WallDirection == 1 && transform.localScale.x > 0)
        {
            _rb2D.velocity = new Vector2(0, _rb2D.velocity.y);
        }
        else if (_touchingDetection.WallDirection == 1 && transform.localScale.x < 0)
        {
            _rb2D.velocity = new Vector2(_moveInput.x * CurrentSpeed, _rb2D.velocity.y);
        }

        if (!_touchingDetection.IsOnWall)
        {
            _rb2D.velocity = new Vector2(_moveInput.x * CurrentSpeed, _rb2D.velocity.y);
        }
    }
}