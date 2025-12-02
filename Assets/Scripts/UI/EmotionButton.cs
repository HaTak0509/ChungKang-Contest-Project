using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using UnityEngine.EventSystems;
using TMPro;

public class EmotionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //*************************************************************
    // [ 코드 설명 ] :
    // 변경할 감정 선택 담당 및, 감정 효과 설명
    // 버튼에 마우스 포인터가 닿으면 설명창을 띄움. 
    //*************************************************************


    [Header("매니저에서 찾을 함수 이름")]
    public string managerMethodName = "TryApplyEmotion";
    public EmotionType type; //이 버튼에 할당된 감정

    public TMP_Text EmotionText { get; private set; }
    public TMP_Text EmotionLore { get; private set; }
    public GameObject Lore { get; private set; }

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

        button.onClick.AddListener(LoreReset);

        Lore = transform.Find("Lore").gameObject;

        EmotionText = Lore.transform.Find("EmotionText").GetComponent<TMP_Text>();
        EmotionLore = Lore.transform.Find("EmotionLore").GetComponent<TMP_Text>();
        Lore.SetActive(false);
    }



    public void OnPointerEnter(PointerEventData eventData) //버튼에 진입시  [현재 감정] + [추가할 감정]표시, 이후 해당 합성 감정의 설명 받아오기
    {
        Lore.SetActive(true);
        LoreReset();
    }

    private void LoreReset()
    {
        var curEmtion = PlayerEmotionController.Instance.CheckCurEmotion();

        EmotionText.text = Emotion.Get(curEmtion).korean + " + " + Emotion.Get(type).korean;


        var emotion = EmotionTable.Mix(curEmtion, type);

        EmotionLore.text = Emotion.Get(emotion).lore;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Lore.SetActive(false);
    }
}

