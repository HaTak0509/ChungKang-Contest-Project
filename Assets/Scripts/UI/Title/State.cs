using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    [SerializeField] private StageManager stageManager;
    [SerializeField] private GameObject upNone;
    [SerializeField] private GameObject collect;
    [SerializeField] private GameObject downNone;

    private void Awake()
    {
        if (stageManager.currentLevel == 0)
        {
            upNone.SetActive(false);
        }
        else if (stageManager.currentLevel == LevelManager.Instance.saveMaxLevel - 1)
        {
            downNone.SetActive(false);
        }
        else
        {
            upNone.SetActive(true);
            collect.SetActive(true);
            downNone.SetActive(true);
        }
    }
}
