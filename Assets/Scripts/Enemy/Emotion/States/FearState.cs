using TMPro;
using UnityEngine;

public class FearState : IEmotionState
{
    //*************************************************************
    // [ 코드 설명 ] :
    // 감정에 따른 여러 행동방식을 유사한 구조로 작성해서, 유지보수와
    // 확장성을 챙기기 위해 인터페이스 형식을 사용함
    //*************************************************************

    public EmotionType Type => EmotionType.Fear;
    
    private MonsterMovement _movement;
    private Transform _player;
    private LayerMask playerLayer;


    private float safeDistance = 4f;  // 플레이어와의 최소 거리
    private float moveSpeed = 0.3f;   // 몬스터가 이동할 기본 속도


    public void OnEnter(Monster monster)
    {
        _movement = monster.GetComponent<MonsterMovement>();
        
        if (_player == null)
            _player = GameObject.FindWithTag("Player").transform;


        if (playerLayer == 0)
            playerLayer = 1 << LayerMask.NameToLayer("Player");
    }

    public void UpdateState(Monster monster)
    {
        if (_player == null) return;

        float distance = Vector2.Distance(monster.transform.position, _player.position);

        if (distance < safeDistance)
        {
            // 플레이어의 반대 방향으로 이동
            float dir = monster.transform.position.x > _player.position.x ? 1f : -1f;

            _movement.Move(dir);

            return;
        }

    }

    public void OnExit(Monster monster) 
    {

    }
}