using UnityEngine;

public class Items : MonoBehaviour
{
    [SerializeField] private ItemManager itemManager;

    [Header("이름 Text")]
    [SerializeField, TextArea(3, 10)] private string nameText;

    [Header("설명 Text")]
    [SerializeField, TextArea(3, 10)] private string contentText;

    public string NameText => nameText;
    public string ContentText => contentText;

    public void OnButtonClick()
    {
        itemManager.OnItemClicked(this);
    }
}
