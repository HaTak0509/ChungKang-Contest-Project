using UnityEngine;



//*************************************************************
// [ 코드 설명 ] :
// 감정이 가지는 속성 정리
//*************************************************************

[CreateAssetMenu(fileName = "EmotionData", menuName = "Game/Emotion Data")]
public class EmotionData : ScriptableObject
{
    public EmotionType type;
    [TextArea] public string korean;
    [TextArea] public string lore;
    [TextArea] public string hexColor;
    public Sprite Sprite;
}