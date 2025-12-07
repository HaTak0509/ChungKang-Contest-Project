

using UnityEngine;
using System;

public class SadState : IEmotionState
{
    //*************************************************************
    // [ 코드 설명 ] :
    // 감정에 따른 여러 행동방식을 유사한 구조로 작성해서, 유지보수와
    // 확장성을 챙기기 위해 인터페이스 형식을 사용함
    //*************************************************************

    public EmotionType Type => EmotionType.Sad;
    private GameObject SmokeParticle;

    private MonsterMovement _movement;

    public void OnEnter(Monster monster)
    {
        _movement = monster.GetComponent<MonsterMovement>();

    }

    public void UpdateState(Monster monster)
    {

        // 1) 앞으로 이동
        _movement.Move(0);

        // 2) 벽이면 반전
        if (_movement.IsWallAhead())
        {
            _movement.Flip();
        }




    }

    public void OnExit(Monster monster) 
    {
    
    }
}