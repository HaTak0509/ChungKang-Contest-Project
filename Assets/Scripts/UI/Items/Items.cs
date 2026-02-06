using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Items : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemContentText;

    [Header("이름 Text")]
    [SerializeField] private TextMeshProUGUI nameText;

    [Header("설명 Text")]
    [SerializeField] private TextMeshProUGUI contentText;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            itemNameText.text = nameText.text;
            itemContentText.text = contentText.text;
        }
    }
}
