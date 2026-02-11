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

    private LevelManager _levelManager;
    private int _currentLevel;

    private void Awake()
    {
        _levelManager = LevelManager.Instance;
        _currentLevel = _levelManager.saveMaxLevel - 1;
    }

    private void Start()
    {
        if (_levelManager == null) return;
        if (_currentLevel < 0 || _currentLevel > stageData.Count) return;

        ShowLevel();
    }

    public void ShowLevel()
    {
        StageData data = stageData[_currentLevel];

        stageName.text = data.stageName;
        stageDescription.text = data.description;
        image.sprite = data.icon;
    }

    public void UpdateLevel(int newLevel)
    {
        if (newLevel < 0 || newLevel >= stageData.Count)
            return;

        if (_currentLevel == newLevel)
            return;

        _currentLevel = newLevel;
        ShowLevel();
    }

}
