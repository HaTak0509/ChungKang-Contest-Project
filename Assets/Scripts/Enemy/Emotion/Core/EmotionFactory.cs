using UnityEngine;
using System.Collections.Generic;
using System;

public static class EmotionFactory
{
    //*************************************************************
    // [ 코드 설명 ] :
    // 몬스터에게 감정을 부여하는 역할을 하도록 하는 코드(임시)
    //
    // [ 주의점 ] :
    // 해당 클랫는 감정 주입 이벤트를 '발행(Invoke)'하는 역할만 함
    // 실제 구독은 Monster.cs가 진행함. | 자세한 부분은 Monster에서
    // 이 클래스에서 이벤트 구독이나 구독해제를 하지 않도록 주의
    //*************************************************************


    private static readonly Dictionary<EmotionType, IEmotionState> _StateTable = new();

    static EmotionFactory() //정적 생성자, 한번만 실행됨. 미리 행동 로직을 딕셔너리에 캐싱해둠
    {
        _StateTable.Add(EmotionType.Joy,new JoyState());
        _StateTable.Add(EmotionType.Sad, new SadState());
        _StateTable.Add(EmotionType.Rage, new RageState());

        _StateTable.Add(EmotionType.Madness, new MadnessState());
        //_StateTable.Add(EmotionType.Joy, new JoyState());
        //_StateTable.Add(EmotionType.Joy, new JoyState());


        //이후 더 추가할 예정
    }

    public static IEmotionState Create(EmotionType type)//현재 상태를 받아옴
    {

        if (_StateTable.ContainsKey(type)) //딕셔너리가 있다면 반환 
        {
            return _StateTable[type];
        }



        // 특정 감정에 대한 State 클래스가 구현되지 않았을 경우 Neutral 상태를 반환, 오류를 방지
        //만약 _StateTable에 Neutral 감정 상태가 등록되어 있으면(= true) 이후 neutralState여기에 넣어서 아래코드 실행
        {
            if (_StateTable.TryGetValue(EmotionType.Null, out var neutralState)) 
            Debug.LogError($"EmotionState not implemented for type: {type}. Returning NeutralState.");
            return neutralState;
        }

        // NeutralState조차 없다면 심각한 오류!!!!!!!!!!!!!!!!!!!!
        throw new Exception($"Fatal Error: EmotionState not implemented: {type} and NeutralState is missing.");


    }
}
