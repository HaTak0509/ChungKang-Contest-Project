using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Items : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private ItemManager itemManager;

    [Header("이름 Text")]
    [SerializeField, TextArea(3, 10)] private string nameText;

    [Header("설명 Text")]
    [SerializeField, TextArea(3, 10)] private string contentText;

    public string NameText => nameText;
    public string ContentText => contentText;

    public void OnPointerClick(PointerEventData eventData)
    {
        itemManager.OnItemClicked(this);
    }
}
