using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set;}

    [SerializeField] private LevelDatabase database;
    [SerializeField] private FadeInFadeOut fadeInOut;
    [SerializeField] private AudioSource TitleBGM;
    [SerializeField] private AudioSource GameBGM;

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

        DontDestroyOnLoad(gameObject);

        saveMaxLevel = SaveLevelManager.Instance.LoadLevel();
        LoadLevel(0);
    }


    public async void OnReset()
    {
        await FadeInFadeOut.instance.StageReset();
    }


    public void LoadLevel(int i)
    {
        CheckBGM();

        if (_currentLevel != null)
            Destroy(_currentLevel);

        _index = i;
        currentLevelIndex = i;
        _currentLevel = Instantiate(database.levels[_index]);

        if (currentLevelIndex > saveMaxLevel)
        {
            saveMaxLevel = currentLevelIndex;
            SaveLevelManager.Instance.SaveLevel(saveMaxLevel);
        }
    }

    private void CheckBGM()
    {
        Debug.Log("이거 진짜에요?");
        if(currentLevelIndex == 0)
        {
            if(!TitleBGM.isPlaying) 
                TitleBGM.Play();
            GameBGM.Stop();
        }
        else
        {

            TitleBGM.Stop();
            if (!GameBGM.isPlaying)
                GameBGM.Play();
        }
    }
}
