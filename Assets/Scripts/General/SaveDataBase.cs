using UnityEngine;

public class SaveDataBase : MonoBehaviour
{
    public static SaveDataBase Instance { get; set;}

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void OnLevelSave()
    {
        PlayerPrefs.SetInt("SaveLevel", LevelReset.Instance._currentLevelIndex);
        PlayerPrefs.Save();
    }

    public void OnLevelLoad()
    {
        LevelReset.Instance._currentLevelIndex = PlayerPrefs.GetInt("SaveLevel");
    }
}
