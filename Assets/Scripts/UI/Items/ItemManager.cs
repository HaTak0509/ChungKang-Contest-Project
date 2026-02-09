using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private GameObject itemPopup;
    [SerializeField] private Image itemImage;
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
        itemImage = clickedItem.ItemImage;
        itemNameText.text = clickedItem.NameText;
        itemContentText.text = clickedItem.ContentText;
    }

    private void Clear()
    {
        currentItem = null;
        itemImage = default;
        itemNameText.text = DefaultName;
        itemContentText.text = DefaultContent;
    }
}
