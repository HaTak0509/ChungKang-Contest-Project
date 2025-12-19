using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class Monster : MonoBehaviour
{
    //*************************************************************
    // [ 코드 설명 ] :
    // 감정에 따른 여러 행동방식을 자연스럽게 연결하고 확장하기위해
    // 행동 로직을 받아오는 구조로 함
    //*************************************************************


    [SerializeField]
    private List<EmotionInventory> _emotionInventories = new List<EmotionInventory>(2);
    public IReadOnlyList<EmotionInventory> EmotionInventories => _emotionInventories;// 외부에서는 이 프로퍼티를 통해 읽기만 가능합니다.

    [SerializeField] public EmotionType _CurrentEmotion { get; private set; } // 읽기 가능, 수정 불가능


    public IEmotionState _currentState { get; private set; }//현재 상태

    public bool _IsOff { get; private set; } = false;

    void Start()
    {

        // 시작 시 초기 감정 설정 | 이부분 보안 필요할지도
        _CurrentEmotion = _emotionInventories[0].Emotion;
        SetEmotion(_CurrentEmotion);

        
    }

    //감정 세팅
    public void SetEmotion(EmotionType newEmotion)//감정에 따른 행동 양식 받아오기
    {
        if (_currentState != null)//진짜 만약에 NULL이 떴다면, 그건 너가 문제야 알겠어?
            _currentState.OnExit(this); // 감정 종료 시 호출


        // [보강] EmotionFactory를 통해 캐싱된 상태 객체를 가져옴 (new를 사용하지 않음)
        _currentState = EmotionFactory.Create(newEmotion); // 새 행동 받아오기 
        Debug.Log($"[Monster] 감정이 {newEmotion.ToString()}으로 변경됨");

        if( _currentState != null )
            _currentState.OnEnter(this); // 감정 변경 시 호출
    }

    private void Update()
    {
        if (!_IsOff)
            _currentState?.UpdateState(this); // 현재 행동이 있다면, 행동 함수 실행
    }

    public void AddEmotion(int index, EmotionType emotionType) //슬롯에 감정 추가하기
    {
        EmotionInventory temp = _emotionInventories[index];
        temp.Emotion = emotionType;
        _emotionInventories[index] = temp; //슬롯에 감정 넣기


        //감정 불러오기
        EmotionType emotion1 = _emotionInventories[0].Emotion;
        EmotionType emotion2 = _emotionInventories[1].Emotion; //슬롯2 가 없을 경우
        if (_emotionInventories != null && _emotionInventories.Count > 1)
        {
            emotion2 = _emotionInventories[1].Emotion;
        }


        SetEmotion(EmotionTable.Mix(emotion1, emotion2)); //슬롯 1,2 합성해서 행동로직 가져오기
    }


}