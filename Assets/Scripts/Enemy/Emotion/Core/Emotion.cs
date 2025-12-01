using System.Collections.Generic;
public enum EmotionType
{
    //*************************************************************
    // [ 코드 설명 ] :
    // 몬스터 감정
    // Neutral의 경우 Null 방지를 위해 추가함
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
    private static readonly Dictionary<EmotionType, string> _EmotionLore = new();

    static Emotion() //정적 생성자, 한번만 실행됨. 미리 행동 로직을 딕셔너리에 캐싱해둠
    {
        _EmotionLore.Add(EmotionType.Joy, "");
        _EmotionLore.Add(EmotionType.Sad, "");
        _EmotionLore.Add(EmotionType.Rage, "");
        _EmotionLore.Add(EmotionType.Fear, "");
        _EmotionLore.Add(EmotionType.HeartBreaking, "");
        _EmotionLore.Add(EmotionType.Jealousy, "");
        _EmotionLore.Add(EmotionType.Thrill, "");
        _EmotionLore.Add(EmotionType.Resentment, "");
        _EmotionLore.Add(EmotionType.Anxiety, "");
        _EmotionLore.Add(EmotionType.Tension, "");
        _EmotionLore.Add(EmotionType.Madness, "");
        _EmotionLore.Add(EmotionType.Screaming, "");
        _EmotionLore.Add(EmotionType.Outrage, "");
        _EmotionLore.Add(EmotionType.Panic, "");
    }

}