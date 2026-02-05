using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] LevelDatabase database;

    public static LevelManager Instance { get; private set;}

    public int currentLevelIndex;
    public int saveMaxLevel;

    private GameObject _currentLevel;
    private int _index;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        saveMaxLevel = SaveLevelManager.Instance.LoadLevel();
        LoadLevel(0);
    }


    public void OnReset()
    {
        LoadLevel(currentLevelIndex);
    }

    public void LoadLevel(int i)
    {
        if (_currentLevel != null)
            Destroy(_currentLevel);

        _index = i;
        currentLevelIndex = i;
        _currentLevel = Instantiate(database.levels[_index]);

        if (currentLevelIndex > saveMaxLevel)
        {
            Debug.Log(saveMaxLevel);
            saveMaxLevel = currentLevelIndex;
            SaveLevelManager.Instance.SaveLevel(saveMaxLevel);
        }

    }
}
