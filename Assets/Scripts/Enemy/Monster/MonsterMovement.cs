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
    public MovementState _movementState { get; private set; } = MovementState.Move;




    public Vector2 pointA;
    public Vector2 pointB;
    public LayerMask collisionLayer;

    private float _direction = 1f; // 1은 오른쪽, -1은 왼쪽
    public bool _isFacingRight { get; private set; } = true;

    void Awake()
    {
        _touchingDetection = GetComponent<TouchingDetection>();
        _monster = GetComponent<Monster>();
        _rb = GetComponent<Rigidbody2D>();


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

        CheckBoundaries();

        if (IsWallAhead())
        {
            _movementState = MovementState.Idle;
        }
        else
        {
            _movementState=MovementState.Move;
        }
    }

    public void Dash()
    {
        Vector2 nextPosition = _rb.position + new Vector2(_monster.Speed * 2 * _direction, 0) * Time.fixedDeltaTime;
        _rb.MovePosition(nextPosition);
    }

    public void ChangeState(MovementState movementState)
    {
        _movementState = movementState;
    }

    public bool IsWallAhead()
    {
        float dir = _isFacingRight ? 1f : -1f;

        Vector2 origin = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            Vector2.right * dir,
            0.6f,
            collisionLayer
        );

        return hit.collider != null;
    }

    public void Flip()
    {
        _direction *= -1;
        _isFacingRight = !_isFacingRight;


        // 방향 전환 시 속도를 즉시 0으로 해주면 더 깔끔하게 꺾입니다.
        StopMove();

        
        GetComponent<SpriteRenderer>().flipX = _direction == 1 ? false : true;

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
    }

    private bool OutBoundaries()
    {
        // 오른쪽으로 가고 있는데 B지점을 넘어섰다면
        if (_direction > 0 && transform.position.x >= pointB.x)
        {
            return true;
        }
        // 왼쪽으로 가고 있는데 A지점을 넘어섰다면
        else if (_direction < 0 && transform.position.x <= pointA.x)
        {
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        // 에디터에서 범위를 보기 쉽게 표시
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector2(pointA.x, transform.position.y - 0.5f), new Vector2(pointA.x, transform.position.y + 0.5f));
        Gizmos.DrawLine(new Vector2(pointB.x, transform.position.y - 0.5f), new Vector2(pointB.x, transform.position.y + 0.5f));
        Gizmos.DrawLine(new Vector2(pointA.x, transform.position.y), new Vector2(pointB.x, transform.position.y));
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
      
        if (_movementState == MovementState.Dash)
        {
            // 플레이어와 접촉 시 밀치기
            if (collision.transform.CompareTag("Player"))
            {
                Debug.Log("플레이어와 충돌");
                collision.transform.GetComponent<Damageable>().TakePushFromPosition(transform.position);
                _movementState = MovementState.Idle;
            }
            
            if (_touchingDetection.IsOnWall || OutBoundaries())
            {
                StopMove();
                _movementState = MovementState.Idle;
            }

        }


    }

}
