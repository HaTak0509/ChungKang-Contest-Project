using System;
using UnityEngine;

public class MonsterEmotionManager : MonoBehaviour
{
    
    //*************************************************************
    // [ 코드 설명 ] :
    // 몬스터의 감정을 변경함
    // 이벤트 구조로 감정을 변경함. 유지보수성및 확장성 좋음
    //*************************************************************
    
    public static Action<Monster, EmotionType> OnEmotionAppliedToMonster;

    public void Awake()
    {
       
    }

    public static void HandleEmotionApplied(Monster monster, EmotionType addEmotion) // 플레이어가 호출하는 감정 변경 함수
    {
        
        var finalEmotion = EmotionTable.Mix(monster.CurrentEmotion, addEmotion);//현재감정, 추가할 감정의 합성 여부를 확인

        if (finalEmotion != monster.CurrentEmotion) //현재감정과 겹치지 않는다면
        {
            Debug.Log($"감정 합성 결과: {monster.CurrentEmotion} + {addEmotion} = {finalEmotion}. 행동 방식 변경됨");
            monster.SetEmotion(finalEmotion);
        }
        else
        {
            Debug.Log($"감정 합성 결과: {monster.CurrentEmotion} + {addEmotion} = {finalEmotion}. 변화 없음");
        }
    }
}