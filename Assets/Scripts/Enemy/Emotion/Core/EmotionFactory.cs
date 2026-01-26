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
    //*************************************************************



    public static IEmotionState Create(EmotionType type)//현재 상태를 받아옴
    {

        return type switch
        {
            EmotionType.Joy => new JoyState(),
            EmotionType.Rage => new RageState(),
            EmotionType.Jealousy => new JealousyState(),
            EmotionType.Resentment => new ResentmentState(),


            EmotionType.Screaming => new ScreamiingState(),
            EmotionType.Outrage => new OutrageState(),




            _ => new NeutralState()
        };



        // NeutralState조차 없다면 심각한 오류!!!!!!!!!!!!!!!!!!!!
        throw new Exception($"Fatal Error: EmotionState not implemented: {type} and NeutralState is missing.");


    }
}
