using System.Collections.Generic;
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
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddItem(ItemData data)
    {
        inventory.Add(data);
        CreateItemButton(data);
    }

    private void CreateItemButton(ItemData data)
    {
        GameObject obj = Instantiate(itemButtonPrefab, content);

        Button btn = obj.GetComponent<Button>();
        Image iconImage = obj.GetComponentInChildren<Image>();

        iconImage.sprite = data.icon;

        btn.onClick.AddListener(() => { FindObjectOfType<ItemManager>().OnItemClicked(data); });
    }
}
