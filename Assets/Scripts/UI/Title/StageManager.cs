using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance {get; private set;}

    [SerializeField] private List<StageData> stageData;
    [SerializeField] private TextMeshProUGUI stageName;
    [SerializeField] private TextMeshProUGUI stageDescription;
    [SerializeField] private List<EnemyUI> enemies;


    public int currentLevel;

    private LevelManager _levelManager;

    private void Awake()
    {
        Instance = this;

        _levelManager = LevelManager.Instance;
        currentLevel = _levelManager.saveMaxLevel - 1;
    }

    private void Start()
    {
        if (_levelManager == null) return;
        if (currentLevel < 0 || currentLevel >= stageData.Count) return;

        ShowLevel();
    }

    public void ShowLevel()
    {
        StageData data = stageData[currentLevel];

        stageName.text = data.stageName;
        stageDescription.text = data.description;

        ShowEnemies();
    }

    public void UpdateLevel(int newLevel)
    {
        int maxSelectableIndex = _levelManager.saveMaxLevel - 1;

        if (newLevel < 0 || newLevel >= stageData.Count || newLevel > maxSelectableIndex) return;

        if (currentLevel == newLevel)
            return;

        currentLevel = newLevel;
        ShowLevel();
    }

    public void ShowEnemies()
    {
        StageData data = stageData[currentLevel];

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].SetActive(false);
        }

        int activeIndex = data.enemyName.Count - 1;

        if (activeIndex >= 0 && activeIndex < enemies.Count)
        {
            enemies[activeIndex].SetActive(true);

            List<string> namesToShow = data.enemyName.GetRange(0, activeIndex + 1);
            enemies[activeIndex].SetNames(namesToShow);
        }
    }

}
