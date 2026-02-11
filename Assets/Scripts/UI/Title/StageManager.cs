using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class StageManager : MonoBehaviour
{
    [SerializeField] private List<StageData> stageData;
    [SerializeField] private TextMeshProUGUI stageName;
    [SerializeField] private TextMeshProUGUI stageDescription;
    [SerializeField] private Image image;

    public int currentLevel;

    private LevelManager _levelManager;

    private void Awake()
    {
        _levelManager = LevelManager.Instance;
        currentLevel = _levelManager.saveMaxLevel - 1;
    }

    private void Start()
    {
        if (_levelManager == null) return;
        if (currentLevel < 0 || currentLevel > stageData.Count) return;

        ShowLevel();
    }

    public void ShowLevel()
    {
        StageData data = stageData[currentLevel];

        stageName.text = data.stageName;
        stageDescription.text = data.description;
        image.sprite = data.icon;
    }

    public void UpdateLevel(int newLevel)
    {
        if (newLevel < 0 || newLevel >= stageData.Count)
            return;

        if (currentLevel == newLevel)
            return;

        currentLevel = newLevel;
        ShowLevel();
    }

}
