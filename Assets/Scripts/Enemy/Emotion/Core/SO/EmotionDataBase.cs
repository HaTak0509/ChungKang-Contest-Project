using System.Collections.Generic;
using UnityEngine;

//*************************************************************
// [ 코드 설명 ] :
// 감정들을 딕셔너리에 모아서 저장
// 이후 Get명령어를 통해 원하는 감정의 세부 정보를 반환함
//*************************************************************

[CreateAssetMenu(fileName = "EmotionDatabase", menuName = "Game/Emotion Database")]
public class EmotionDatabase : ScriptableObject
{
    public List<EmotionData> emotions = new();

    private Dictionary<EmotionType, EmotionData> _dict;

    public EmotionData Get(EmotionType type) 
    {
        if (_dict == null)
        {
            _dict = new();
            foreach (var e in emotions)
            {
                if (!_dict.ContainsKey(e.type))
                    _dict.Add(e.type, e);
            }
        }

        return _dict[type];
    }
}