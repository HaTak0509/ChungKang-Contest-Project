using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetItems : MonoBehaviour
{
    [SerializeField] private GameObject itemButtons;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Transform content;
    public int items;

    private void Start()
    {
        for (int i = 0; i <= items; i++)
        {
            GameObject obj = Instantiate(itemButtons, content);

            Button btn = obj.GetComponent<Button>();
            TMP_Text text = obj.GetComponentInChildren<TMP_Text>();

            text.text = $"¾ÆÀÌÅÛ{i}";

            int index = i;
            btn.onClick.AddListener(() => { Items.Instance.OnButtonClick(); });
        }
    }
}
