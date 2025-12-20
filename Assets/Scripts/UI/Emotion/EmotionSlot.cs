using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EmotionSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    //*************************************************************
    // [ 코드 설명 ] :
    // 몬스터의 감정 슬롯, 감정의 드롭 감지, 필터가 있는지 검수함
    //*************************************************************

    private Monster curMonster;
    private Outline _outline;

    public int slotIndex;

    private void Start()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
        curMonster = transform.root.GetComponent<Monster>(); //부모인 몬스터 불러오기
        if(curMonster == null)
        {
            Debug.LogWarning("아, 부모에 Monster 없다던데? 야 따로 빈 오브젝트 넣었니?");
        }

    }


    public void OnDrop(PointerEventData eventData)//OnEndDrag보다 빠르게 호출
    {
        // eventData.pointerDrag는 현재 드래그 중인 오브젝트입니다.
        if (eventData.pointerDrag != null)
        {
            //무사히 슬롯에 안착 했으면 bool값 변화
            EmotionSprite emotionSprite = eventData.pointerDrag.GetComponent<EmotionSprite>();
            emotionSprite.isDropped = true;
            if (CheckFilter(emotionSprite.Type))
            {
                if (transform.childCount != 0)//만약 이미 감정이 있다면, 해당 감정 제거
                    return;

                if(emotionSprite.monster != null)
                {
                    emotionSprite.Remove();
                }

                // 아이템의 위치를 슬롯 위치로 고정
                eventData.pointerDrag.transform.SetParent(transform, false);
                eventData.pointerDrag.GetComponent<RectTransform>().transform.localPosition = Vector3.zero;

                //몬스터에 들어감 상태로 변경 및 해당 몬스터 기억
                emotionSprite.ChaingedState(EmotionSprite.SlotState.Monster);
                emotionSprite.monster = curMonster;

                curMonster.AddEmotion(slotIndex, emotionSprite.Type); //슬롯에 감정을 넣고, 합성인지 확인
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<EmotionSprite>())
        {
            _outline.enabled = true;
        }
    }
        public void OnPointerExit(PointerEventData eventData) => _outline.enabled = false;

    bool CheckFilter(EmotionType emotionType) //필터가 있는지 확인하고, 필터가 있다면 확인
    {
        List<EmotionType> types = curMonster.EmotionInventories[slotIndex].Filter;

        if (types == null || types.Count == 0)
        {
            Debug.Log("필터 없음");
            return true;
        }

        for (int i = 0; i < types.Count; i++)
        {
            if (types[i] == emotionType)
            {
                Debug.Log("필터에 성립함");
                return true;

            }
        }
        Debug.Log("필터에 들어가지 않음");
        return false;
    }
}
