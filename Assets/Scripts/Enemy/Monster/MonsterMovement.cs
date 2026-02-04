using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    //*************************************************************
    // [ 코드 설명 ] :
    // 몬스터의 움직임, 경로설정, 돌진등을 감지함
    //
    // 이동은 사전에 지정된 두개의 A,B의 지점을 왕복하는 방식으로 진행되며
    // 만약 이동방향에 오브젝트를 만난다면 FSM상태를 대기로 변경 후 정지
    // 돌진또한 Dash 상태를 변경하는 방식으로 진행됨
    //*************************************************************


    public enum MovementState
    {
        Idle,
        Move,
        Dash
    }


    private Rigidbody2D _rb;
    private TouchingDetection _touchingDetection;
    private Monster _monster;
    private Transform _player;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    public MovementState _movementState { get; private set; } = MovementState.Move;




    public Vector2 pointA;
    public Vector2 pointB;

    private float _direction = 1f; // 1은 오른쪽, -1은 왼쪽
    public bool _isFacingRight { get; private set; } = true;

    void Awake()
    {
        _touchingDetection = GetComponent<TouchingDetection>();
        _monster = GetComponent<Monster>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        // 시작 시 A와 B 중 더 오른쪽에 있는 값을 자동으로 정렬 (실수 방지)
        if (pointA.x > pointB.x)
        {
            Vector2 temp = pointA;
            pointA = pointB;
            pointB = temp;
        }
        if (_player == null)
            _player = GameObject.FindWithTag("Player").transform;
    }

    public void Move()
    {
        if (_movementState == MovementState.Move)
        {
            Vector2 nextPosition = _rb.position + new Vector2(_monster.Speed * _direction, 0) * Time.fixedDeltaTime;

            _rb.MovePosition(nextPosition);
        }

        if (_movementState == MovementState.Move)
            _animator.SetBool(AnimationStrings.IsMoving, true);
        else _animator.SetBool(AnimationStrings.IsMoving, false);

        CheckBoundaries();

        if (_touchingDetection.WallDirection != 0)
        {
            _movementState = MovementState.Idle;
        }
        else
        {
            _movementState=MovementState.Move;
        }
    }

    public void Dash(float Speed)
    {
        _direction = GetDirectionToPlayer();

        _isFacingRight = _direction > 0 ? true : false ;
        _spriteRenderer.flipX = _direction == 1 ? false : true;

        if (_direction == _touchingDetection.WallDirection)
        {
            Speed = 0;
        }

        Vector2 nextPosition = _rb.position + new Vector2(Speed * _direction, 0) * Time.fixedDeltaTime;
        _rb.MovePosition(nextPosition);
    }

    public int GetDirectionToPlayer()
    {
        if (_player == null) return 0; // 타겟이 없으면 0

        // 플레이어 X - 몬스터 X
        float diff = _player.position.x - transform.position.x;

        // 양수면 오른쪽(1), 음수면 왼쪽(-1)
        return diff > 0 ? 1 : -1;
    }

    public void ChangeState(MovementState movementState)
    {
        _movementState = movementState;
    }


    public void Flip()
    {
        _direction *= -1;
        _isFacingRight = !_isFacingRight;


        // 방향 전환 시 속도를 즉시 0으로 해주면 더 깔끔하게 꺾입니다.
        StopMove();


        _spriteRenderer.flipX = _direction == 1 ? false : true;

    }

    public void StopMove()
    {
        _rb.velocity = new Vector2(0, _rb.velocity.y);
    }

    private void CheckBoundaries()
    {
        // 오른쪽으로 가고 있는데 B지점을 넘어섰다면
        if (_direction > 0 && transform.position.x >= pointB.x)
        {
            Flip();
        }
        // 왼쪽으로 가고 있는데 A지점을 넘어섰다면
        else if (_direction < 0 && transform.position.x <= pointA.x)
        {
            Flip();
        }

        // 현재 위치가 설정한 사각형 범위를 벗어났는지 체크 (여유값 0.5f 추가)
        bool outOfX = transform.position.x < pointA.x - 0.5f || transform.position.x > pointB.x + 0.5f;

        if (outOfX)
        {
            StopMove();
        }
    }


    private void OnDrawGizmos()
    {
        // 에디터에서 범위를 보기 쉽게 표시
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector2(pointA.x, transform.position.y - 0.5f), new Vector2(pointA.x, transform.position.y + 0.5f));
        Gizmos.DrawLine(new Vector2(pointB.x, transform.position.y - 0.5f), new Vector2(pointB.x, transform.position.y + 0.5f));
        Gizmos.DrawLine(new Vector2(pointA.x, transform.position.y), new Vector2(pointB.x, transform.position.y));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("ActivationCrack"))
        {

        }
    }

}
