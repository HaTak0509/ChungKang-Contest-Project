using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stage/StageData")]

public class StageData : ScriptableObject
{
    public List<string> enemyName;
    public string stageName;

    [TextArea(3, 10)]
    public string description;
}
