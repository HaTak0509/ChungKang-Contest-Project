using System.Collections.Generic;
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

    [Header("기본 설정")]
    public float baseSpeed = 5f;
    public float baseDetectionRange = 5f;

    // 현재 실제 적용되는 값
    public float Speed { get; private set; }
    public float InteractRange { get; private set; }


    // 나를 멈추게 하는 원인들
    private HashSet<string> disableReasons = new HashSet<string>();

    public bool IsDisabled => disableReasons.Count > 0;


    [SerializeField]
    public EmotionType _emotion = EmotionType.Null;
    public EmotionType _CurrentEmotion { get; private set; } // 읽기 가능, 수정 불가능

    public IEmotionState _currentState { get; private set; } = null;//현재 상태

    void Start()
    {
        InteractRange = baseDetectionRange;


        // 시작 시 초기 감정 설정 | 이부분 보안 필요할지도
        _CurrentEmotion = _emotion;
        SetEmotion(_CurrentEmotion);
    }


    public void SetEmotion(EmotionType newEmotion)//감정에 따른 행동 양식 받아오기
    {
        if (_currentState != null)//진짜 만약에 NULL이 떴다면, 그건 너가 문제야 알겠어?
            _currentState.OnExit(this); // 감정 종료 시 호출



        // [보강] EmotionFactory를 통해 캐싱된 상태 객체를 가져옴 (new를 사용하지 않음)
        _currentState = EmotionFactory.Create(newEmotion); // 새 행동 받아오기 
        Debug.Log($"[Monster] 감정이 {newEmotion.ToString()}으로 변경됨");

        _CurrentEmotion = newEmotion;


        if (_currentState != null)
            _currentState.OnEnter(this); // 감정 변경 시 호출
    }

    private void FixedUpdate()
    {

        Speed = IsDisabled ? 0f : baseSpeed;
        InteractRange = IsDisabled ? 0f : baseDetectionRange;


        _currentState?.UpdateState(this); // 현재 행동이 있다면, 행동 함수 실행

    }

    public void TwistMob()
    {
        EmotionType twist_temp = Emotion.Twist(_CurrentEmotion);

        SetEmotion(twist_temp);
    }


    private void OnDrawGizmos()
    {
        // 에디터에서 범위를 보기 쉽게 표시
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y), InteractRange);
    }

}