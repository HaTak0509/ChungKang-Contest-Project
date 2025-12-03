using UnityEngine;

public class PlayerEmotionController : MonoBehaviour
{
    //*************************************************************
    // [ 코드 설명 ] :
    // 적을 클릭하면 감정 패널 호출, 이후 감정 변경 상태임을 Bool로 확인
    // 이후 1,2,3,4또는 버튼 클릭으로 감정 변경 요청
    //
    // [ 주의점 ] :
    // 해당 클랫는 감정 주입 이벤트를 '발행(Invoke)'하는 역할만 함
    // 실제 구독은 Monster.cs가 진행함. | 자세한 부분은 Monster에서
    // 이 클래스에서 이벤트 구독이나 구독해제를 하지 않도록 주의
    //*************************************************************

    private Camera _mainCamera;
    private bool _IsControlling = false;
    private Monster _CurMonster;

    public static PlayerEmotionController Instance;

    void Awake()
    {
        Instance = this;
        _mainCamera = Camera.main; //메인카메라 캐싱
       
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckEmotion();
        }

        if (_IsControlling)
        {

            if (Input.GetKeyDown(KeyCode.Alpha1)) // 1번 키 → 기쁨 주입
                TryApplyEmotion(EmotionType.Joy);

            if (Input.GetKeyDown(KeyCode.Alpha2)) // 2번 키 → 슬픔
                TryApplyEmotion(EmotionType.Sad);

            if (Input.GetKeyDown(KeyCode.Alpha3)) // 3번 키 → 화남
                TryApplyEmotion(EmotionType.Rage);

            if (Input.GetKeyDown(KeyCode.Alpha4)) // 4번 키 → 두려움
                TryApplyEmotion(EmotionType.Fear);
        }
    }

    void CheckEmotion()
    {
        if (_mainCamera == null)
        {
            Debug.LogError("메인카메라 캐싱 안됨. 고쳐오셈");
            return;
        }

        Debug.Log("클릭 시도");

        // 마우스 클릭 위치 월드 좌표 변환
        Vector2 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            Debug.Log("몬스터 발견");
            _CurMonster = hit.collider.GetComponent<Monster>();

            if(!_IsControlling)
                HandUIController.Instance.InUI();
            _IsControlling = _CurMonster;
            
        }
        else
        {
            Debug.Log("몬스터 없음");
        }
    }

    public void TryApplyEmotion(EmotionType emotion)
    {

        if (_CurMonster != null)
        {
            Debug.Log($"감정 ({emotion}) 주입 시도");

            // MonsterEmotionManager의 정적 이벤트를 호출
            if (MonsterEmotionManager.OnEmotionAppliedToMonster != null)
            {
                MonsterEmotionManager.OnEmotionAppliedToMonster.Invoke(_CurMonster, emotion);

                // [ 진행 방향 ] 
                // 플레이어가 몬스터의 감정 변경을 시도함
                // 여기서 Invoke -> Monster에서 OnEmotionApplied호출 ->
                // MonsterEmotionManager에 HandleEmotionApplied호출됨
                // 몬스터의 감정이 변경됨
            }
            else
            {
                Debug.LogWarning("MonsterEmotionManager의 이벤트 리스너가 연결되지 않았습니다.");
            }
        }
    }

    public EmotionType CheckCurEmotion()
    {
        if (_CurMonster != null)
            return _CurMonster.CurrentEmotion;
        else
            return EmotionType.Neutral;
    }
}