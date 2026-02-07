using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set;}

    [SerializeField] private LevelDatabase database;
    [SerializeField] private FadeInFadeOut fadeInOut;

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


    public async void OnReset()
    {

        await FadeInFadeOut.instance.StageReset();
    }


    public void LoadLevel(int i)
    {
        if (_currentLevel != null)
            Destroy(_currentLevel);

        _index = i;
        currentLevelIndex = i;
        _currentLevel = Instantiate(database.levels[_index]);

        if(currentLevelIndex != 0)
        FadeInFadeOut.instance.FadeOut(); //이거 자연스러운 맵 전환을 위해 추가함

        if (currentLevelIndex > saveMaxLevel)
        {
            saveMaxLevel = currentLevelIndex;
            SaveLevelManager.Instance.SaveLevel(saveMaxLevel);
        }
    }
}
