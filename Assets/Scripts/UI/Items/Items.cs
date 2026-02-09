using UnityEngine;
using UnityEngine.UI;

public class Items : MonoBehaviour
{
    public static Items Instance {get; private set;}

    [Header("그림 Image")]
    [SerializeField] private Image itemImage;

    [Header("이름 Text")]
    [SerializeField, TextArea(3, 10)] private string nameText;

    [Header("설명 Text")]
    [SerializeField, TextArea(3, 10)] private string contentText;

    private ItemManager itemManager;

    public Image ItemImage => itemImage;
    public string NameText => nameText;
    public string ContentText => contentText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        itemManager = FindObjectOfType<ItemManager>();
    }

    public void OnButtonClick()
    {
        itemManager.OnItemClicked(this);
    }
}
