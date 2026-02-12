using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // 이벤트 시스템 필수!

public class ButtonHoverEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image _Image;
    public TMP_Text _Text;
    public Color _TextColor;

    private void Awake()
    {
        _Image = GetComponent<Image>();
    }

    // 마우스가 버튼 영역 안으로 들어왔을 때 실행
    public void OnPointerEnter(PointerEventData eventData)
    {
        Color color = _Image.color;
        color.a = 1;
        _Image.color = color;

        _Text.color = _TextColor;
    }

    // 마우스가 버튼 영역 밖으로 나갔을 때 실행
    public void OnPointerExit(PointerEventData eventData)
    {
       SetLeave();
    }

    public void SetLeave()
    {
        Color color = _Image.color;
        color.a = 0;
        _Image.color = color;

        _Text.color = Color.white;
    }

}