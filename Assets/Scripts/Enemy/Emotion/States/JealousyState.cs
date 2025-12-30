using UnityEngine;
public class JealousyState : IEmotionState
{
    //*************************************************************
    // [ 코드 설명 ] :
    // 감정에 따른 여러 행동방식을 유사한 구조로 작성해서, 유지보수와
    // 확장성을 챙기기 위해 인터페이스 형식을 사용함
    //*************************************************************

    public EmotionType Type => EmotionType.Jealousy;


    private MonsterMovement _movement;
    private Transform _player;

    private float _TeleportTimer = 0f;      // 텔레포트 대기 타이머 (3초)
    private float _CoolTimer = 0f;      // 텔레포트 쿨타임 (몰루 /초)
    private LayerMask _MonsterLayer = LayerMask.GetMask("Enemy");

    private bool _isTeleporting = false;
    private bool _isCooltime = false;


    private const float Ready_DURATION = 3f;
    private const float Cool_DURATION = 15f;
    private const float checkRadius = 3f;

    public void OnEnter(Monster monster)
    {
        if (_movement == null)
            _movement = monster.GetComponent<MonsterMovement>();

        if (_player == null)
            _player = GameObject.FindWithTag("Player").transform;

        StartTP();
    }

    public void UpdateState(Monster monster)
    {
        
        if (_isCooltime)
        {
            _CoolTimer -= Time.deltaTime;
            if (_CoolTimer <= 0)
            {
                StartTP();
            }
        }

        _CoolTimer -= Time.deltaTime;

        if (_isTeleporting)
        {
            _TeleportTimer -= Time.deltaTime;
            if (_TeleportTimer <= 0)
            {
                Teleport();
            }

        }
        else
        {
            _movement.Move();
        }


    }

    void StartTP()
    {
        Debug.Log("텔포 시작!");
        _isTeleporting = true;
        _isCooltime = false;
        _TeleportTimer = Ready_DURATION;
    }

    void Teleport()
    {
        // 1. 주변 AI 탐색
        Collider2D[] hitAIs = Physics2D.OverlapCircleAll(_player.transform.position, checkRadius, _MonsterLayer);

        if (hitAIs.Length == 0) return; // 3블럭 이내 AI가 없으면 종료

        Transform targetAI = null;
        float minDistance = float.MaxValue;

        foreach (var hit in hitAIs)
        {
            if(hit.gameObject == this._movement.gameObject) 
                continue;


            float distance = Vector2.Distance(_player.transform.position, hit.transform.position);

            if (targetAI == null || distance < minDistance)
            {
                minDistance = distance;
                targetAI = hit.transform;
            }
            // 거리가 같은 경우 x값이 더 큰(오른쪽) AI 선택
            else if (Mathf.Approximately(distance, minDistance))
            {
                if (hit.transform.position.x > targetAI.position.x)
                {
                    targetAI = hit.transform;
                }
            }
        }

        if (targetAI != null)
        {
            Vector3 myPos = _movement.transform.position;
            _movement.transform.position = targetAI.position;
            targetAI.position = myPos;
        }

        _CoolTimer = Cool_DURATION;
        Debug.Log("텔포 종료");
        _isCooltime = true;
        _isTeleporting = false;
    }



    public void OnExit(Monster monster) 
    {

    }
}