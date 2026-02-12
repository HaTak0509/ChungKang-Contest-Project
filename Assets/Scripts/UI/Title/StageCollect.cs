using UnityEngine;

public class StageCollect : MonoBehaviour
{
    public void CollectStage()
    {
        if (StageManager.Instance == null) return;

        LevelManager.Instance.LoadLevel(StageManager.Instance.currentLevel + 1);
    }
}
