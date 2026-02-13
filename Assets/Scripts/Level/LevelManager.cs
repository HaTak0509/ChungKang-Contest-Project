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

    private int _curLever = -1;
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

    private void Update()
    {
        if (_curLever != currentLevelIndex)
        {
            _curLever = currentLevelIndex;
            CheckBGM();
        }

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
            saveMaxLevel = currentLevelIndex;
            SaveLevelManager.Instance.SaveLevel(saveMaxLevel);
        }
    }

    private void CheckBGM()
    {


        Debug.Log("이거 진짜에요?");
        if(_curLever == 0)
        {
            if(!TitleBGM.isPlaying) 
                TitleBGM.Play();
            GameBGM.Stop();
        }
        else
        {

            if (!GameBGM.isPlaying)
                GameBGM.Play();
            TitleBGM.Stop();
        }
        if (currentLevelIndex >= 9)
        {
            GameBGM.Stop();
            TitleBGM.Stop();
        }
    }
}
