using UnityEngine;

public class Monster : MonoBehaviour
{
    //*************************************************************
    // [ 코드 설명 ] :
    // 감정에 따른 여러 행동방식을 유사한 구조로 작성해서, 유지보수와
    // 확장성을 챙기기 위해 인터페이스 형식을 사용함
    //*************************************************************


    [Header("Emotion State")]
    [SerializeField] private EmotionType _initialEmotion = EmotionType.Neutral; // 몬스터의 기본 시작 감정
    [SerializeField] public EmotionType CurrentEmotion { get; private set; } // 읽기 가능, 수정 불가능

    [SerializeField] public EmotionType OriginEmotion { get; private set; } // 읽기 가능, 수정 불가능
    [SerializeField] public EmotionType? PlusEmotion { get; private set; } // 읽기 가능, 수정 불가능



    private IEmotionState _currentState;
    private bool _IsOff = false;
    void Start()
    {
        // 이벤트 리스너 등록: MonsterEmotionManager의 정적 이벤트를 구독, 다른 곳에서 감정 주입 요청이 오면 처리함
        MonsterEmotionManager.OnEmotionAppliedToMonster += OnEmotionApplied;

        // 시작 시 초기 감정 설정 | 이부분 보안 필요할지도
        OriginEmotion = _initialEmotion;
        SetEmotion(OriginEmotion);

        
    }

    void OnDestroy()
    {
        //메모리 누수 방지를 위해 오브젝트 제거시 이벤트 구독 해제
        MonsterEmotionManager.OnEmotionAppliedToMonster -= OnEmotionApplied;
    }

    // 이벤트 핸들러: '나'에게 감정 주입 요청이 들어왔는지 확인하고 처리
    //해당 코드는 OnEmotionAppliedToMonster.Invoke가 호출 되면 얘가 불러와짐 | 옵저버 구조
    private void OnEmotionApplied(Monster targetMonster, EmotionType addEmotion)     
    {
        if (targetMonster == this)
        {
            MonsterEmotionManager.HandleEmotionApplied(this, addEmotion);
        }
    }

    //감정 세팅
    public void SetEmotion(EmotionType newEmotion)
    {
        if (_currentState != null)//진짜 만약에 NULL이 떴다면, 그건 너가 문제야 알겠어?
            _currentState.OnExit(this); // 감정 종료 시 호출

        if (OriginEmotion == EmotionType.Neutral) //만약 시작 감정이 무감정이었다면 감정으로 변경
        {
            Debug.Log("시작감정이 무감정");
            OriginEmotion = newEmotion;
        }

        // [보강] EmotionFactory를 통해 캐싱된 상태 객체를 가져옴 (new를 사용하지 않음)
        _currentState = EmotionFactory.Create(newEmotion); // 새 행동 받아오기 
        CurrentEmotion = newEmotion; // 감정 변경
        Debug.Log($"[Monster] 감정이 {newEmotion.ToString()}으로 변경됨");

        _currentState.OnEnter(this); // 감정 변경 시 호출
    }

    public void RemoveEmotion()
    {
        if (PlusEmotion != null) //합성감정이라면
        {
            PlusEmotion = null;

            SetEmotion(OriginEmotion);

        }
        else//기본 감정이라면
        {
            _IsOff = true;
            Debug.Log("해옹 OFF");
        }
    }


    public void AddEmotion(EmotionType emotion)
    {
        PlusEmotion = emotion;
    }


    private void Update()
    {
        if (!_IsOff)
            _currentState?.UpdateState(this); // 현재 행동이 있다면, 행동 함수 실행
    }
}