using UnityEngine;

public class LevelReset : MonoBehaviour
{
    [SerializeField] LevelDatabase database;

    public static LevelReset Instance;

    private GameObject currentLevel;
    private int index;

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
    public void LoadLevel(int i)
    {
        if (currentLevel != null)
            Destroy(currentLevel);

        index = i;
        currentLevel = Instantiate(database.levels[index]);
    }
}
