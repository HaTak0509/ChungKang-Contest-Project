using System.Collections.Generic;
using UnityEngine;
public enum EmotionType
{
    //*************************************************************
    // [ 코드 설명 ] :
    // 몬스터 감정
    // Neutral의 경우 Null 방지를 위해 추가함
    // 정적 생성자로, 감정에따른 설명을 저장하고자, 딕셔너리에 초기화
    //*************************************************************



    Null, //NULL

    //기본 검정
    Joy, //기쁨
    Rage, //화남
    Jealousy, //질투
    Resentment, //원망
    Screaming, //절규
    Outrage, //격분



    //뒤틀린 감정
    TwistJoy,//기쁨
    TwistRage, //화남
    TwistResentment, //원망
    TwistScreaming, //절규
    TwistOutrage, //격분(절망)

}

public static class Emotion
{

    private static EmotionDatabase _db; //감정목록

    private static readonly Dictionary<EmotionType, EmotionType> _mixTable =
        new()
        {

            {EmotionType.Joy, EmotionType.TwistJoy},
            {EmotionType.Rage, EmotionType.TwistRage},
            {EmotionType.Jealousy, EmotionType.Jealousy },
            {EmotionType.Resentment, EmotionType.TwistResentment },
            {EmotionType.Screaming, EmotionType.TwistScreaming},
            {EmotionType.Outrage, EmotionType.TwistOutrage},

            //반전된 감정들
            {EmotionType.TwistJoy, EmotionType.Joy},
            {EmotionType.TwistRage, EmotionType.Rage},
            {EmotionType.TwistResentment, EmotionType.Resentment },
            {EmotionType.TwistScreaming, EmotionType.Screaming},
            {EmotionType.TwistOutrage, EmotionType.Outrage}
        };


    public static EmotionDatabase DB
    {
        get
        {
            if (_db == null) //캐싱이 안됐다면?
                _db = Resources.Load<EmotionDatabase>("EmotionDatabase"); //Assets/Resources/경로를 따라 찾음.
           
            
            return _db;
        }
    }

    public static EmotionType Twist(EmotionType type)
    {
        return _mixTable[type];
    }


    public static EmotionData Get(EmotionType type) //우리가 사용하는 부분, 감정을 넣으면 해당 감정에 속성을 모두 가져옴
    {
        return DB.Get(type); //EmotionDataBase의 GEt() 실행하여 속성 가져옴

    }
}

