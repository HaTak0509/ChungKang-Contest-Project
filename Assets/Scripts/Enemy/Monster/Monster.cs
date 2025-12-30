using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Monster : MonoBehaviour
{
    //*************************************************************
    // [ 코드 설명 ] :
    // 몬스터의 감정 로직을 총괄하는 스크립트
    //
    // 몬스터는 감정을 새로 받으면 그에 맞는 행동 로직을 
    // EmotuonFactory에서 받아오고
    // Fiexed업데이트에서 매프레임 실행한다
    //*************************************************************

    public float InteractRange = 5f;


    [SerializeField]
    private List<EmotionInventory> _emotionInventories = new List<EmotionInventory>(2);
    public IReadOnlyList<EmotionInventory> EmotionInventories => _emotionInventories;// 외부에서는 이 프로퍼티를 통해 읽기만 가능합니다.

    [field: SerializeField] public EmotionType _CurrentEmotion { get; private set; } // 읽기 가능, 수정 불가능


    public IEmotionState _currentState { get; private set; } = null;//현재 상태
    public EmotionPannelController _controller;

    private MonsterMovement _movement;
    private Coroutine _OffCoroutine;

    void Start()
    {
        _movement = GetComponent<MonsterMovement>();
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

        _CurrentEmotion = newEmotion;

        _controller.EmotionTextChange(Emotion.Get(_CurrentEmotion).korean);

        if( _currentState != null )
            _currentState.OnEnter(this); // 감정 변경 시 호출
    }

    private void FixedUpdate()
    {


        _currentState?.UpdateState(this); // 현재 행동이 있다면, 행동 함수 실행

    }

    public void AddEmotion(int index, EmotionType emotionType) //슬롯에 감정 추가하기
    {
        EmotionInventory temp = _emotionInventories[index];
        temp.Emotion = emotionType;
        _emotionInventories[index] = temp; //슬롯에 감정 넣기


        //감정 불러오기
        EmotionType emotion1 = _emotionInventories[0].Emotion;
        EmotionType emotion2 = EmotionType.Null;
        if (_emotionInventories != null && _emotionInventories.Count > 1)
        {
            emotion2 = _emotionInventories[1].Emotion;
        }


        SetEmotion(EmotionTable.Mix(emotion1, emotion2)); //슬롯 1,2 합성해서 행동로직 가져오기
    }

    public void StartWait(float Time)
    {
        if (_OffCoroutine == null)
            _OffCoroutine = StartCoroutine(WaitOff(Time));
    }
    
    private IEnumerator WaitOff(float Wait)
    {
        float Origin_Inter = InteractRange;
        float Origin_Speed = _movement.Speed;

        InteractRange = 0;
        _movement.Speed = 0;

        yield return new WaitForSeconds(Wait);

        InteractRange = Origin_Inter;
        _movement.Speed = Origin_Speed;
    }



    private void OnDrawGizmos()
    {
        // 에디터에서 범위를 보기 쉽게 표시
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y), InteractRange);
    }

}