using UnityEngine;

[CreateAssetMenu(menuName = "Stage/StageData")]

public class StageData : ScriptableObject
{
    public Sprite icon;
    public string stageName;

    [TextArea(3, 10)]
    public string description;
}
