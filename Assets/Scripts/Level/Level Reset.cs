using UnityEngine;

public class LevelReset : MonoBehaviour
{
    [SerializeField] LevelDatabase database;

    public static LevelReset Instance { get; private set;}

    public int _currentLevelIndex;

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

        LoadLevel(0);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadLevel(0);
        }
    }

    public void OnReset()
    {
        LoadLevel(_currentLevelIndex);
    }

    public void LoadLevel(int i)
    {
        if (_currentLevel != null)
            Destroy(_currentLevel);

        _index = i;
        _currentLevelIndex = i;
        _currentLevel = Instantiate(database.levels[_index]);
        // 여기서 레벨 저장
    }
}
