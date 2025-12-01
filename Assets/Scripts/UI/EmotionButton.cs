using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using UnityEngine.EventSystems;

public class EmotionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //*************************************************************
    // [ 코드 설명 ] :
    // 변경할 감정 선택 담당 및, 감정 효과 설명
    //*************************************************************


    [Header("매니저에서 찾을 함수 이름")]
    public string managerMethodName = "TryApplyEmotion";
    public EmotionType type;

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();

        // 매니저 인스턴스 가져오기
        var manager = PlayerEmotionController.Instance;

        if (manager == null)
        {
            Debug.LogError("PlayerEmotionController 인스턴스를 찾을 수 없습니다.");
            return;
        }

    
        MethodInfo method = manager.GetType().GetMethod(managerMethodName,     // 리플렉션으로 함수 찾기
            BindingFlags.Public | BindingFlags.Instance);

        //manager.GetType()은 객체의 실제 타입을 가져옴
        //managerMethodName 이름의 매서드를 착음
        //BindingFlags.Public | BindingFlags.Instance 이는 Public 이고, Instance 메서드(static이 아닌 메서드)만 찾음

        if (method == null)
        {
            Debug.LogError($"{managerMethodName} 함수를 PlayerEmotionController 찾을 수 없습니다.");
            return;
        }

        // 버튼 클릭에 해당 함수 실행하도록 연결
        button.onClick.AddListener(() =>
        {
            method.Invoke(manager, new object[] { type });
        });
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("마우스가 버튼 위에 들어옴");
        // 원하는 동작 (소리 재생 / 색 변경 / 강조 등)
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("마우스가 버튼에서 나감");
        // Hover 효과 제거 등
    }
}

