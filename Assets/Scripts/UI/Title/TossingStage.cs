using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TossingStage : MonoBehaviour
{
    private StageManager _stageManager;

    private void Awake()
    {
        _stageManager = StageManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            _stageManager.UpdateLevel(_stageManager.currentLevel - 1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            _stageManager.UpdateLevel(_stageManager.currentLevel + 1);
        }
    }
}
