using System.Collections.Generic;
using UnityEngine;

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