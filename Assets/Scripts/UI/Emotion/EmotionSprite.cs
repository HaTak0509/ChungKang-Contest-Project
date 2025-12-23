using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EmotionSprite : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    //*************************************************************
    // [ 코드 설명 ] :
    // 감정, 드래그 및 드랍 관리
    //*************************************************************

    public Sprite Sprite;
    public EmotionType Type;
    [HideInInspector] public bool isDropped;
    [HideInInspector] public Monster monster;


    public Vector2 originalPosition { get; private set; }
    public SlotState curSlot { get; private set; } = SlotState.Inventory;
    public enum SlotState
    {
        Inventory,
        Monster
    }


    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Vector2 offset; // 마우스와 UI 중심 사이의 차이



    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();


        GetComponent<Image>().sprite = Emotion.Get(Type).Sprite;

       
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 드래그 시작 시 투명도 조절 및 마우스 클릭 통과 설정
        canvasGroup.alpha = 0.4f;
        canvasGroup.blocksRaycasts = false;
        originalPosition = rectTransform.anchoredPosition;
        isDropped = false;

        // 클릭한 지점(screenPoint)을 RectTransform 내부 좌표로 변환하여 오프셋 계산
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint);

        offset = localPoint;

    }

    public void OnDrag(PointerEventData eventData)
    {
        // 현재 마우스 위치를 다시 로컬 좌표로 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localMousePos);

        // 오프셋을 적용하여 위치 설정 (커서가 잡은 위치 그대로 유지)
        rectTransform.localPosition = localMousePos - offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그 종료 시 초기화
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;


        if (curSlot == SlotState.Monster && isDropped == false)//몬스터 슬롯에 있는데, 드롭이 다른곳이라면?
        {
            if (PlayerEmotionInventory.Instance.transform.childCount >= PlayerEmotionInventory.Instance.InventoryCount)
            {
                PlayerEmotionInventory.OnErrorPannel.Invoke("빈 인벤토리가 없습니다.");
                rectTransform.anchoredPosition = originalPosition;
                return;
            }

            Remove();

            //위치 변경
            transform.SetParent(PlayerEmotionInventory.Instance.Inventory, false);
            rectTransform.transform.localPosition = Vector3.zero;

            ChaingedState(SlotState.Inventory);


            return;
        }


      
        // 특정 슬롯에 놓이지 않았다면 원래 위치로 복귀
        rectTransform.anchoredPosition = originalPosition;

    }

    public void Remove()
    {
        Debug.Log("몬스터인데, 딴데 떨어짐");

        monster.AddEmotion(GetComponentInParent<EmotionSlot>().slotIndex, EmotionType.Null); //슬롯에 감정을 넣고, 합성인지 확인
        monster = null;
    }


    public void ChaingedState(SlotState state)//슬롯에 대한 정보 변경
    {
        curSlot = state;
    }
}