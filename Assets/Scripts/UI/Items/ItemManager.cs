using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private GameObject itemPopup;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemContentText;
    [SerializeField] private bool isTitle;
 
    private ItemData currentItem;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !isTitle)
        {
            itemPopup.SetActive(!itemPopup.activeSelf);
        }
    }

    public void OnItemClicked(ItemData data)
    {
        if (currentItem == data)
        {
            Clear();
            return;
        }

        currentItem = data;

        itemImage.sprite = data.icon;
        itemNameText.text = data.itemName;
        itemContentText.text = data.description;
    }

    private void Clear()
    {
        currentItem = null;

        itemImage.sprite = null;
        itemNameText.text = "아이템 이름";
        itemContentText.text = "아이템 설명";
    }
}
