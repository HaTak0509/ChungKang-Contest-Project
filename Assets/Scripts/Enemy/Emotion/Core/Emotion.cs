using UnityEngine;
public enum EmotionType
{
    //*************************************************************
    // [ 코드 설명 ] :
    // 몬스터 감정
    // Neutral의 경우 Null 방지를 위해 추가함
    // 정적 생성자로, 감정에따른 설명을 저장하고자, 딕셔너리에 초기화
    //*************************************************************



    Neutral, //NULL

    //기본 검정
    Joy, //기쁨
    Sad, //슬픔
    Rage, //화남
    Fear, //두려움 

    // 합성 감정
    HeartBreaking, //애틋
    Jealousy, //질투
    Thrill, //설렘
    Resentment, //원망
    Anxiety, //불안
    Tension, //긴장
    Madness, //광기
    Screaming, //절규
    Outrage, //격분
    Panic//패닉
}



public static class Emotion
{

    private static EmotionDatabase _db; //감정목록

    public static EmotionDatabase DB
    {
        get
        {
            if (_db == null) //캐싱이 안됐다면?
                _db = Resources.Load<EmotionDatabase>("EmotionDatabase"); //Assets/Resources/경로를 따라 찾음.
           
            
            return _db;
        }
    }

    public static EmotionData Get(EmotionType type) //우리가 사용하는 부분, 감정을 넣으면 해당 감정에 속성을 모두 가져옴
    {
        return DB.Get(type); //EmotionDataBase의 GEt() 실행하여 속성 가져옴

    }
}

