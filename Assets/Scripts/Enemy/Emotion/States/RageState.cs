
using Cinemachine.Utility;
using UnityEngine;
using static MonsterMovement;

public class RageState : Monster
{
    //*************************************************************
    // [ 코드 설명 ] :
    // RAGE의 경우 FSM을 바탕으로 감지하고, 돌진하고, 복귀함

    //*************************************************************

    public Transform _player;

    // 로직 제어 변수
    [SerializeField] private float DashSpeed = 5f;
    [SerializeField] private Vector3 _HitPos = new Vector2(0.0f, 0.0f);
    [SerializeField] private Vector2 _HitCheck = new Vector2(0.5f, 0.5f);
    [SerializeField] private LayerMask _PlayerLayer;


    private bool _isDashing = false;           // 현재 돌진 중인지 여부


    public override void OnEnter()
    {
        _movement._player = _player;
        _lineRenderer.OnLine();

        if (ColorUtility.TryParseHtmlString(hexColor, out Color newColor))
        {
            Debug.Log(newColor.ToString());
            _lineRenderer.Draw(InteractRange, newColor);
        }

    }

    public override void UpdateState()
    {

        if (_isDashing)
        {
            _movement.Dash(DashSpeed);
            CheckHit();
        }
        else
        {
            _movement.Move();
            if (IsPlayerInRange())
                _isDashing = true;
        }
    }

    public override void OnExit()
    {
        _lineRenderer.OffLine();
        StopDash();
    }


    private void StopDash()
    {
        Debug.Log("돌진 끝");
        _isDashing = false;
        _movement.StopMove();
    }

    private void CheckHit()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position + _HitPos, _HitCheck, 0f, _PlayerLayer);
        // 플레이어와 접촉 시 밀치기

        foreach (Collider2D hit in hits)
        {
            if (hit.transform.CompareTag("Player"))
            {
                Debug.Log("플레이어와 충돌");

                _animator.SetTrigger("isAction");
                _movement.ChangeState(MovementState.Idle);
                StopDash();

                hit.transform.GetComponent<Damageable>().GameOver();

            }
        }
    }

    private bool IsPlayerInRange()
    {
        float dist = Vector3.Distance(transform.position, _player.position);
        return dist <= InteractRange;
    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(transform.position + _HitPos, _HitCheck);
    }
}
