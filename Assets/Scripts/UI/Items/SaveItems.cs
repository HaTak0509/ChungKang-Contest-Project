using UnityEngine;

public class SaveItems : MonoBehaviour
{
    public static SaveItems Instance { get; private set; }

    private const string Item_Key = "SaveItems";

    private void Awake()
    {
        Instance = this;
    }

    public void SaveItem(int itemIndex)
    {
        PlayerPrefs.SetInt(Item_Key, itemIndex);
        PlayerPrefs.Save();
    }

    public int LadeItem()
    {
        return PlayerPrefs.GetInt(Item_Key, 0);
    }
}
