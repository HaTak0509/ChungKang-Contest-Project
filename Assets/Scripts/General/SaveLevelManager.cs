using UnityEngine;
public class SaveLevelManager : MonoBehaviour
{
    public static SaveLevelManager Instance { get; private set; }

    private const string LEVEL_KEY = "SaveLevel";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void SaveLevel(int levelIndex)
    {
        PlayerPrefs.SetInt(LEVEL_KEY, levelIndex);
        PlayerPrefs.Save();
    }

    public int LoadLevel()
    {
        return PlayerPrefs.GetInt(LEVEL_KEY, 1);
    }
}
