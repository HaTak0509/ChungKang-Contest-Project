using UnityEngine;

[CreateAssetMenu(fileName = "EmotionData", menuName = "Game/Emotion Data")]
public class EmotionData : ScriptableObject
{
    public EmotionType type;
    [TextArea] public string korean;
    [TextArea] public string lore;
}