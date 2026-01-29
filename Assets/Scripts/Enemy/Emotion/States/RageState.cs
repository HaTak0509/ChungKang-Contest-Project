using UnityEngine;

public class RageState : Monster
{
    //*************************************************************
    // [ 코드 설명 ] :
    // RAGE의 경우 FSM을 바탕으로 감지하고, 돌진하고, 복귀함

    //*************************************************************

    private Transform _player;

    // 로직 제어 변수
    private float _attackStateTimer = 0f;      // 공격 상태 유지 타이머 (3초)
    private float _readyTimer = 0f;            // 돌격 준비 타이머 (1초)
    private bool _isDashing = false;           // 현재 돌진 중인지 여부
    private bool _isReadying = false;          // 현재 준비 중인지 여부

    private const float ATTACK_DURATION = 3f;
    private const float READY_DURATION = 1f;

    public override void OnEnter()
    {

        if (_player == null)
            _player = GameObject.FindWithTag("Player").transform;


        _lineRenderer.OnLine();

        if (ColorUtility.TryParseHtmlString(hexColor, out Color newColor))
        {
            Debug.Log(newColor.ToString());
            _lineRenderer.Draw(InteractRange, newColor);
        }

    }

    public override void UpdateState()
    {
        // 1. 플레이어 감지 시 유지 시간 갱신
        if (IsPlayerInRange())
        {
            _attackStateTimer = ATTACK_DURATION;
        }

        // 2. 공격 상태 타이머 체크
        _attackStateTimer -= Time.deltaTime;
        if (_attackStateTimer <= 0)
        {
            StopDash();
            return;
        }

        // 3. 내부 동작 로직 (준비 -> 돌진)
        HandleAttackLogic();
    }

    public override void OnExit()
    {
        _lineRenderer.OffLine();
        StopDash();
    }

    private void HandleAttackLogic()
    {
        if (!_isReadying && !_isDashing)
        {
            // 준비 단계 진입
            StartReady();
        }

        if (_isReadying)
        {
            _readyTimer -= Time.deltaTime;
            // 준비 시간 동안 플레이어 위치 추적 (목표 갱신)


            if (_readyTimer <= 0)
            {
                StartDash();
            }
        }

        if (_isDashing)
        {
            _movement.Dash();
        }
        

        if (_isDashing && _movement._movementState != MonsterMovement.MovementState.Dash)
        {
            StopDash();
        }

    }

    private void StartReady()
    {
        _isReadying = true;
        _isDashing = false;
        _readyTimer = READY_DURATION;
        Debug.Log("돌진 준비");

    }

    private void StartDash()
    {

        _isReadying = false;
        _isDashing = true;
        _movement.ChangeState(MonsterMovement.MovementState.Dash);


        //플레이어 위치 기준 위/ 아래 방향 판단
        float xDirection = Mathf.Sign(_player.position.x - transform.position.x);

        // ▶ 몬스터가 플레이어 방향을 바라보도록 Flip
        if (_player.position.x < transform.position.x && _movement._isFacingRight)
        {
            _movement.Flip();
        }
        else if (_player.position.x > transform.position.x && !_movement._isFacingRight)
        {
            _movement.Flip();
        }

        Debug.Log("돌진");
    }

    private void StopDash()
    {
        Debug.Log("돌진 끝");
        _isDashing = false;
        _isReadying = false;
        _movement.StopMove();
    }

    private bool IsPlayerInRange()
    {
        float dist = Vector3.Distance(transform.position, _player.position);
        return dist <= InteractRange;
    }

}