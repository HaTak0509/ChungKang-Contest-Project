using System.Collections.Generic;

public static class EmotionTable
{
    //*************************************************************
    // [ 코드 설명 ] :
    // 감정 합성
    // 딕셔너리에서 관리함으로 관리, 확장하기 쉬움
    //*************************************************************



    //감정 조합표
    private static readonly Dictionary<(EmotionType, EmotionType), EmotionType> _mixTable =
        new()
        {
            {(EmotionType.Joy, EmotionType.Sad), EmotionType.HeartBreaking},
            {(EmotionType.Joy, EmotionType.Rage), EmotionType.Jealousy},
            {(EmotionType.Joy, EmotionType.Fear), EmotionType.Thrill},

            {(EmotionType.Sad, EmotionType.Rage), EmotionType.Resentment},
            {(EmotionType.Sad, EmotionType.Fear), EmotionType.Anxiety},

            {(EmotionType.Rage, EmotionType.Fear), EmotionType.Tension},
            
            {(EmotionType.Joy, EmotionType.Joy), EmotionType.Madness},
            {(EmotionType.Sad, EmotionType.Sad), EmotionType.Screaming},
            {(EmotionType.Rage, EmotionType.Rage), EmotionType.Outrage},
            {(EmotionType.Fear, EmotionType.Fear), EmotionType.Panic},
        };

    public static EmotionType Mix(EmotionType current, EmotionType added) //두감정을 조합해서 나온 결과를 반환하는 함수
    {
        if (_mixTable.ContainsKey((current, added))) //현재 감정 + 추가되는 감정을 _mixTable에 Key에 존재하는 체크하는 것임ㅇㅇ
            return _mixTable[(current, added)]; //있다면 해당 Key의 Value, 즉 합성 감정을 반환

        if (_mixTable.ContainsKey((added, current))) //순서 반대로 해서 검사
            return _mixTable[(added, current)];

        //조합이 없다면 추가되는 감정을 반환, 

        //해당 부분은 보완필요함. 무감정이나, 분해관련해서도 이야기가 필요함
        return added; 
    }

}
