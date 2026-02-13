using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemBase : MonoBehaviour
{
    public static ItemBase Instance { get; private set; }

    [SerializeField] private GameObject itemButtonPrefab;
    [SerializeField] private Transform content;

    private List<ItemData> inventory = new List<ItemData>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadInventory();
    }

    public void AddItem(ItemData data)
    {
        inventory.Add(data);
        inventory.Sort((a, b) => int.Parse(a.itemID).CompareTo(int.Parse(b.itemID)));
        RefreshUI();
        SaveInventory();
    }

    private void CreateItemButton(ItemData data)
    {
        GameObject obj = Instantiate(itemButtonPrefab, content);

        Button button = obj.GetComponent<Button>();
        TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();

        text.text = data.itemName;

        obj.GetComponent<Button>().onClick.AddListener(() =>
        {
            FindObjectOfType<ItemManager>().OnItemClicked(data, button);
        });
    }

    private void RefreshUI()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in inventory)
        {
            CreateItemButton(item);
        }
    }


    private void SaveInventory()
    {
        List<string> ids = new List<string>();

        foreach (var item in inventory)
        {
            ids.Add(item.itemID);
        }

        string json = JsonUtility.ToJson(new IDList(ids));
        PlayerPrefs.SetString("Inventory", json);
        PlayerPrefs.Save();
    }

    private void LoadInventory()
    {
        if (!PlayerPrefs.HasKey("Inventory")) return;

        string json = PlayerPrefs.GetString("Inventory");
        IDList loaded = JsonUtility.FromJson<IDList>(json);

        foreach (string id in loaded.ids)
        {
            ItemData data = ItemDatabase.Instance.GetItem(id);

            if (data != null)
            {
                inventory.Add(data);
                CreateItemButton(data);
            }
        }
    }

    [System.Serializable]
    private class IDList
    {
        public List<string> ids;
        public IDList(List<string> ids) { this.ids = ids; }
    }
}
