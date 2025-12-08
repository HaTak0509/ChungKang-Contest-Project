

using UnityEngine;
using System;

public class JoyState : IEmotionState
{
    //*************************************************************
    // [ 코드 설명 ] :
    // 감정에 따른 여러 행동방식을 유사한 구조로 작성해서, 유지보수와
    // 확장성을 챙기기 위해 인터페이스 형식을 사용함
    //*************************************************************

    public EmotionType Type => EmotionType.Joy;


    private MonsterMovement _movement;

    public void OnEnter(Monster monster)
    {
        if(_movement == null)
            _movement = monster.GetComponent<MonsterMovement>();
    }

    public void UpdateState(Monster monster)
    {

        _movement.Move(0.6f);

        if (_movement.IsWallAhead())
        {
            _movement.Flip();
        }


    }

    public void OnExit(Monster monster) 
    {
    
    }
}