using UnityEngine;
public class JealousyState : Monster, IInteractable
{
    //*************************************************************
    // [ 코드 설명 ] :
    // 감정에 따른 여러 행동방식을 유사한 구조로 작성해서, 유지보수와
    // 확장성을 챙기기 위해 인터페이스 형식을 사용함
    //*************************************************************


    private Transform _player;

    private float _TeleportTimer = 0f;      // 텔레포트 대기 타이머 (3초)

    private bool _isTeleporting = false;


    [Header("대기 시간 설정")]
    public float Ready_DURATION = 5f;
    private const float checkRadius = 99f;



    public override void OnEnter()
    {

        if (_player == null)
            _player = GameObject.FindWithTag("Player").transform;


    }

    public override void UpdateState()
    {

        if (_isTeleporting)
        {
            _TeleportTimer -= Time.deltaTime;
            if (_TeleportTimer <= 0)
            {
                Teleport();
            }

        }

    }

    public void StartTP()
    {
        if (_isTeleporting) return;

        Debug.Log("텔포 시작!");
        _animator.SetTrigger("IsAction");
        _isTeleporting = true;
        _TeleportTimer = Ready_DURATION;
    }

    void Teleport()
    {
        _animator.SetTrigger("IsAction");

        Collider2D[] hitAIs = Physics2D.OverlapCircleAll(_player.transform.position, checkRadius, LayerMask.GetMask("Enemy"));

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

        Debug.Log("텔포 종료");
        _isTeleporting = false;
    }

    public void Interact()
    {
        StartTP();
    }


    public void OnExit(Monster monster) 
    {
        _isTeleporting = false;
        _TeleportTimer = 0;
    }

}