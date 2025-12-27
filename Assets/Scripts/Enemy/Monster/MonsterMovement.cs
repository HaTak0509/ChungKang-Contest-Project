using System.Collections;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{

    public enum MovementState
    {
        Idle,
        Move
    }


    private Rigidbody2D _rb;
    private MovementState _movementState = MovementState.Move;
    //private Monster _monster;



    
    public Vector2 pointA;
    public Vector2 pointB;
    public LayerMask collisionLayer;




    private float _direction = 1f; // 1은 오른쪽, -1은 왼쪽
    public bool _isFacingRight { get; private set; } = true;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        //_monster = GetComponent<Monster>();

        // 시작 시 A와 B 중 더 오른쪽에 있는 값을 자동으로 정렬 (실수 방지)
        if (pointA.x > pointB.x)
        {
            Vector2 temp = pointA;
            pointA = pointB;
            pointB = temp;
        }

    }

    public void Move(float Speed)
    {
        if (_movementState == MovementState.Move)
        {
            _rb.velocity = new Vector2(Speed * _direction, _rb.velocity.y);
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
        _rb.velocity = new Vector2(0, _rb.velocity.y);

        
        GetComponent<SpriteRenderer>().flipX = _direction == 1 ? false : true;

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

    private void OnDrawGizmos()
    {
        // 에디터에서 범위를 보기 쉽게 표시
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector2(pointA.x, transform.position.y - 0.5f), new Vector2(pointA.x, transform.position.y + 0.5f));
        Gizmos.DrawLine(new Vector2(pointB.x, transform.position.y - 0.5f), new Vector2(pointB.x, transform.position.y + 0.5f));
        Gizmos.DrawLine(new Vector2(pointA.x, transform.position.y), new Vector2(pointB.x, transform.position.y));
    }

    


    //private IEnumerator ChargeRoutine(Transform player, float WaitTime = 1f)
    //{
    //    IsChargeWait = true;
    //    yield return new WaitForSeconds(WaitTime);
    //    IsCharge = true;
    //    IsChargeWait = false;

    //    // 플레이어 위치 기준 위/아래 방향 판단
    //    float xDirection = Mathf.Sign(player.position.x - transform.position.x);

    //    // ▶ 몬스터가 플레이어 방향을 바라보도록 Flip
    //    if (player.position.x < transform.position.x)
    //        transform.localScale = new Vector3(-1, 1, 1);
    //    else
    //        transform.localScale = new Vector3(1, 1, 1);

    //    // ▶ 속도 적용 (x축 직선 돌진)
    //    _rb.velocity = new Vector2(xDirection * 15f, 0);

    //    // 돌진 시간
    //    float timer = 0f;
    //    float chargeTime = 0.5f;

    //    while (timer < chargeTime)
    //    {
    //        timer += Time.deltaTime;
    //        yield return null;
    //    }

    //    // 돌진 종료
    //    _rb.velocity = Vector2.zero;
    //    IsCharge = false;

    //}

    //public IEnumerator WaitRoutine(float WaitTime = 2f)
    //{
    //    float timer = 0f;

    //    while (timer < WaitTime)
    //    {
    //        timer += Time.deltaTime;
    //        yield return null;
    //    }

    //    IsChargeCool = true;
    //}

    //public void Stop()
    //{
    //    _rb.velocity = Vector2.zero;
    //}

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.collider.CompareTag("Player") && IsCharge)
    //    {
    //        IsCharge = false;
    //        if (_monster._currentState is RageState rage)
    //        {
    //            _rb.velocity = Vector2.zero;
    //            rage.OnHitPlayer();
    //        }
    //    }
    //    else if (collision.collider.CompareTag("Wall"))
    //    {
    //        _rb.velocity = Vector2.zero;
    //        IsCharge = false;
    //    }
    //}
}
