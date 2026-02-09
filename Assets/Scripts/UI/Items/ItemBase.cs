using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemBase : MonoBehaviour
{
    public static ItemBase Instance {get; private set;}

    [SerializeField] private GameObject itemButtons;
    [SerializeField] private Transform content;

    public int items;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < items; i++)
        {
            CreateItemButton(i);
        }
    }

    private void CreateItemButton(int index)
    {
        GameObject obj = Instantiate(itemButtons, content);

        Button btn = obj.GetComponent<Button>();
        TMP_Text text = obj.GetComponentInChildren<TMP_Text>();

        text.text = $"¾ÆÀÌÅÛ{index}";

        btn.onClick.AddListener(() => { Items.Instance.OnButtonClick(); });
    }

    public void AddItem()
    {
        CreateItemButton(items);
        items++;
    } 
}
