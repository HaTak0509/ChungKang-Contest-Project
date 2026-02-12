using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance;

    [SerializeField] private List<ItemData> items;

    private Dictionary<string, ItemData> itemDict;

    private void Awake()
    {
        Instance = this;

        itemDict = new Dictionary<string, ItemData>();

        foreach (var item in items)
        {
            if (!itemDict.ContainsKey(item.itemID)) 
            {
                itemDict.Add(item.itemID, item);
            }
            else
            {
                Debug.LogError($"중복된 itemID 발견: {item.itemID}");
            }
        }
    }

    public ItemData GetItem(string id)
    {
        if (itemDict.TryGetValue(id, out ItemData data))
            return data;

        return null;
    }
}
