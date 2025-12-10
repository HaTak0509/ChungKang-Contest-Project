using System.Collections;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    private float _direction = 1f; // 1 = 오른쪽, -1 = 왼쪽
    private float moveSpeed = 3f;
    private Rigidbody2D _rb;
    private Coroutine _chargeRoutine;
    private Coroutine _WaitRoutine;
    private Monster _monster;
    private LayerMask wallLayer;

    public float jumpForce = 8f;
   
    
    
    public bool _isFacingRight { get; private set; } = true;
    public bool IsCharge { get; private set; } = false;
    public bool IsChargeWait { get; private set; } = false;
    public bool IsChargeCool = false;


    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _monster = GetComponent<Monster>();
        // 만약 인스펙터에서 지정하지 않았다면 자동 설정
        if (wallLayer == 0)
            wallLayer = 1 << LayerMask.NameToLayer("Wall");
    }

    public void Move(float dir)
    {
        _rb.velocity = new Vector2(_direction * dir * moveSpeed, _rb.velocity.y);

        if (IsWallAhead())
            Flip();

    }
    public bool IsWallAhead()
    {
        float dir = _isFacingRight ? 1f : -1f;

        Vector2 origin = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            Vector2.right * dir,
            0.6f,
            wallLayer
        );

        return hit.collider != null;
    }
    public void Flip()
    {
        _direction *= -1;  // **방향 반전**
        _isFacingRight = !_isFacingRight;
        Vector3 s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
    }


    public void StartCharge(Transform player, float WaitTime = 1f)
    {
        if (_chargeRoutine == null)
            _chargeRoutine = StartCoroutine(ChargeRoutine(player,WaitTime));
    }

    private IEnumerator ChargeRoutine(Transform player, float WaitTime = 1f)
    {
        IsChargeWait = true;
        yield return new WaitForSeconds(WaitTime);
        IsCharge = true;
        IsChargeWait = false;

        // 플레이어 위치 기준 위/아래 방향 판단
        float xDirection = Mathf.Sign(player.position.x - transform.position.x);

        // ▶ 몬스터가 플레이어 방향을 바라보도록 Flip
        if (player.position.x < transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);

        // ▶ 속도 적용 (x축 직선 돌진)
        _rb.velocity = new Vector2(xDirection * 15f, 0);

        // 돌진 시간
        float timer = 0f;
        float chargeTime = 0.5f;

        while (timer < chargeTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // 돌진 종료
        _rb.velocity = Vector2.zero;
        IsCharge = false;

        _chargeRoutine = null;
    }

    public void StartWait(float WaitTime = 2f)
    {
        if (_WaitRoutine == null)
            _WaitRoutine = StartCoroutine(WaitRoutine(WaitTime));
    }

    public IEnumerator WaitRoutine(float WaitTime = 2f)
    {
        float timer = 0f;

        while (timer < WaitTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        IsChargeCool = true;
    }

    public void Stop()
    {
        _rb.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && IsCharge)
        {
            IsCharge = false;
            if (_monster._currentState is RageState rage)
            {
                _rb.velocity = Vector2.zero;
                rage.OnHitPlayer();
            }
        }
        else if (collision.collider.CompareTag("Wall"))
        {
            _rb.velocity = Vector2.zero;
            IsCharge = false;
        }
    }
}