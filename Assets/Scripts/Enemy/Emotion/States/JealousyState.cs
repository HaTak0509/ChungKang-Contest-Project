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
    private LayerMask _playerLayer = LayerMask.GetMask("Player");


    private float _TeleportTimer = 0f;      // 텔레포트 대기 타이머 (3초)
    private float _CoolTimer = 0f;      // 텔레포트 쿨타임 (몰루 /초)

    private bool _isTeleporting = false;

    private const float Ready_DURATION = 3f;
    private const float Cool_DURATION = 15f;


    public void OnEnter(Monster monster)
    {
        if (_movement == null)
            _movement = monster.GetComponent<MonsterMovement>();



        StartTP();
    }

    public void UpdateState(Monster monster)
    {
        _movement.Move();

        _CoolTimer -= Time.deltaTime;

        if (_isTeleporting)
        {
            _TeleportTimer -= Time.deltaTime;
            if (_TeleportTimer <= 0)
            {
                

            }

        }


    }

    void StartTP()
    {
        _isTeleporting = true;
        _TeleportTimer = Ready_DURATION;
    }

    void Teleport()
    {

    }



    public void OnExit(Monster monster) 
    {

    }
}