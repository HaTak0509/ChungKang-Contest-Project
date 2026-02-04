using TMPro;
using UnityEngine;

public class Lore : MonoBehaviour
{
    [SerializeField] private TMP_Text m_Text;

    public void SetText(string text)
    {
        m_Text.text = text;
    }
}
