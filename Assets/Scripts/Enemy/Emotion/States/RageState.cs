
using UnityEngine;

public class RageState : IEmotionState
{
    //*************************************************************
    // [ 코드 설명 ] :
    // RAGE의 경우 FSM을 바탕으로 감지하고, 돌진하고, 복귀함
    
    //*************************************************************

    public EmotionType Type => EmotionType.Rage;

    private MonsterMovement _movement;
    private Transform _monsterTransform;
    private Transform _player;
    private LayerMask playerLayer;

    public enum State { Idle, Charge, Recover } //FSM상태
    public State currentState = State.Idle; //대기

    public float detectRange = 5f; //감지 범위



    public void OnEnter(Monster monster) 
    {
        _movement = monster.GetComponent<MonsterMovement>();
        _monsterTransform = monster.transform;

        if (_player == null)
            _player = GameObject.FindWithTag("Player").transform;

        if (playerLayer == 0)
            playerLayer = 1 << LayerMask.NameToLayer("Player");

        currentState = State.Idle;
    }

    public void UpdateState(Monster monster)
    {
        switch (currentState)
        {
            case State.Idle:
                _movement.Move(0.6f);

                if (IsPlayerInFront())
                {
                    currentState = State.Charge;
                }
                break;

            case State.Charge:
                _movement.StartCharge(_player);

                if(_movement.IsChargeWait == false && _movement.IsCharge == false) 
                    currentState = State.Recover;
                break;
            case State.Recover:
                    
                break;
        }
    }


    public void OnExit(Monster monster)
    {

    }

    private Vector2 GetForward()
    {
        return _movement._isFacingRight ? Vector2.right : Vector2.left;
    }

    private bool IsPlayerInFront()
    {
        Vector2 origin = _monsterTransform.position;
        Vector2 dir = GetForward();

        // 레이캐스트
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, detectRange, playerLayer);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            return true;
        }

        return false;
    }

    public void OnHitPlayer()
    {
        Debug.Log("플레이어와 충돌!");


        // TODO: 데미지 / Recover 상태 전환
        
    }
}