using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private GameObject itemPopup;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemContentText;

    private const string DefaultName = "아이템 이름";
    private const string DefaultContent = "아이템 내용";

    public bool itemActive;
    public Items currentItem;

    private void Update()
    {
        if (itemPopup == null) return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            itemPopup.SetActive(!itemPopup.activeSelf);
        }
    }

    public void OnItemClicked(Items clickedItem)
    {
        if (currentItem == clickedItem)
        {
            Clear();
            return;
        }

        currentItem = clickedItem;
        itemNameText.text = clickedItem.NameText;
        itemContentText.text = clickedItem.ContentText;
    }

    private void Clear()
    {
        currentItem = null;
        itemNameText.text = DefaultName;
        itemContentText.text = DefaultContent;
    }
}
