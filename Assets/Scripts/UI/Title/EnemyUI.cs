using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyUI : MonoBehaviour
{
    public List<TextMeshProUGUI> nameText;

    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }

    public void SetNames(List<string> names)
    {
        for (int i = 0; i < nameText.Count; i++)
        {
            nameText[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < names.Count && i < nameText.Count; i++)
        {
            nameText[i].gameObject.SetActive(true);
            nameText[i].text = names[i];
        }
    }
}
