using UnityEngine;

public class StageCollect : MonoBehaviour
{
    private LevelManager _levelManager;

    private void Awake()
    {
        _levelManager = LevelManager.Instance;
    }

    public void CollectStage()
    {
        if (_levelManager.currentLevelIndex <= _levelManager.saveMaxLevel)
        {
            _levelManager.LoadLevel(_levelManager.currentLevelIndex + 1);
        }
    }
}
