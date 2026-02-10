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
            itemDict.Add(item.itemID, item);
        }
    }

    public ItemData GetItem(string id)
    {
        if (itemDict.TryGetValue(id, out ItemData data))
            return data;

        return null;
    }
}
