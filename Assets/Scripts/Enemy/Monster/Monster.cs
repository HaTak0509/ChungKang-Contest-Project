using System.Collections.Generic;
using UnityEngine;


public class Monster : MonoBehaviour, WarpingInterface
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
    public float baseDetectionRange = 3f;
    public string hexColor = "#FFFFFF";
    public GameObject TwistMonster;
    
    protected MonsterMovement _movement;
    protected Animator _animator;
    protected DrawSensingRange _lineRenderer;


    // 현재 실제 적용되는 값
    public float Speed { get; private set; }
    public float InteractRange { get; private set; }


    // 나를 멈추게 하는 원인들
    private bool _isOnEnter = false;
    private HashSet<string> disableReasons = new HashSet<string>();
    [HideInInspector] public bool _isFirst = false;

    public bool IsDisabled => disableReasons.Count > 0;

    private void Awake()
    {
        _movement = GetComponent<MonsterMovement>();
        _animator = GetComponent<Animator>();
        _lineRenderer = GetComponent<DrawSensingRange>();

        OnEnter();
    }

    void Start()
    {
        

       
        

        if (TwistMonster != null && !_isFirst)
        {
            InteractRange = baseDetectionRange;

            TwistMonster = Instantiate(TwistMonster);
            _isFirst = true;
         
            Monster temp = TwistMonster.GetComponent<Monster>();

            temp._isFirst = true;
            temp.TwistMonster = gameObject;
            temp.baseSpeed = baseSpeed;
            temp.InteractRange = baseDetectionRange;
            TwistMonster.SetActive(false);

            TwistMonster.GetComponent<MonsterMovement>().pointA = _movement.pointA;
            TwistMonster.GetComponent<MonsterMovement>().pointB = _movement.pointB;
        }
    }



    private void FixedUpdate()
    {

        Speed = IsDisabled ? 0f : baseSpeed;
        InteractRange = IsDisabled ? 0f : baseDetectionRange;

        UpdateState();
    }

    public void Warping()
    {
        if (TwistMonster == null) return;
     
        if(!_isOnEnter) OnEnter();
        
        OnExit();

        
        TwistMonster.SetActive(true);

        TwistMonster.transform.position = transform.position;
        TwistMonster.GetComponent<Monster>().OnEnter();

        gameObject.SetActive(false);

    }


    public virtual void OnEnter()
    {
        _isOnEnter = true;
    }

    public virtual void UpdateState()
    {
       
    }
    public virtual void OnExit()
    {

    }


    private void OnDrawGizmos()
    {
        // 에디터에서 범위를 보기 쉽게 표시
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y), baseDetectionRange);
    }

}