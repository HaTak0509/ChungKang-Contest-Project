using UnityEngine;

public class State : MonoBehaviour
{
    [SerializeField] private GameObject upNone;
    [SerializeField] private GameObject collect;
    [SerializeField] private GameObject downNone;

    private LevelManager levelManager;
    private StageManager stageManager;

    private void Awake()
    {
        levelManager = LevelManager.Instance;
        stageManager = StageManager.Instance;
    }

    private void Update()
    {
        if (levelManager.saveMaxLevel == 1)
        {
            upNone.SetActive(false);
            collect.SetActive(true);
            downNone.SetActive(false);
        }
        else if (stageManager.currentLevel == 0 && levelManager.saveMaxLevel != 1)
        {
            upNone.SetActive(false);
            collect.SetActive(true);
            downNone.SetActive(true);
        }
        else if (stageManager.currentLevel == levelManager.saveMaxLevel - 1)
        {
            upNone.SetActive(true);
            collect.SetActive(true);
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
