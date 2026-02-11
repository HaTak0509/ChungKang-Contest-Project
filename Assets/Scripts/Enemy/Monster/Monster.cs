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
    private GameObject twist;

    protected MonsterMovement _movement;
    protected Animator _animator;
    protected DrawSensingRange _lineRenderer;
    protected Rigidbody2D _rb;

    // 현재 실제 적용되는 값
    public float Speed { get; private set; }
    public float InteractRange { get; private set; }


    // 나를 멈추게 하는 원인들
    private HashSet<string> disableReasons = new HashSet<string>();
    [HideInInspector] public bool _isFirst = false;

    public bool IsDisabled => disableReasons.Count > 0;

    private void Awake()
    {
        _movement = GetComponent<MonsterMovement>();
        _animator = GetComponent<Animator>();
        _lineRenderer = GetComponent<DrawSensingRange>();
        _rb = GetComponent<Rigidbody2D>();
        

        InteractRange = baseDetectionRange;
    }

    void Start()
    {
        if (!_isFirst)
        {
            OnEnter();


            if (TwistMonster != null)
            {
                twist = Instantiate(TwistMonster);
                twist.transform.SetParent(transform.parent);
                _isFirst = true;

                Monster temp = twist.GetComponent<Monster>();

                temp._isFirst = true;
                temp.twist = gameObject;
                temp.baseSpeed = baseSpeed;
                temp.Speed = baseSpeed;
                temp.baseDetectionRange = baseDetectionRange;
                temp.InteractRange = baseDetectionRange;

                var targetList = InteractionSequence.instance.sequence;

                if (targetList != null && targetList.Contains(gameObject))
                {


                    int myIndex = targetList.IndexOf(gameObject);

                    targetList.Insert(myIndex + 1, twist);
                }



                twist.GetComponent<MonsterMovement>().pointA = _movement.pointA;
                twist.GetComponent<MonsterMovement>().pointB = _movement.pointB;


                if (twist.GetComponent<CircleCollider2D>() != null)
                {
                    // 부모의 스케일 값 중 가장 큰 값을 가져와서 나눕니다.
                    float currentScale = transform.lossyScale.x;

                    if (currentScale != 0) twist.GetComponent<CircleCollider2D>().radius = InteractRange / currentScale;
                    // 실제 반지름 = 원하는 거리 / 현재 스케일
                }

                twist.SetActive(false);
            }
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
        if (twist == null)
        {
            Debug.Log("이새끼 범인임ㅇㅇ");
            return;
        }

        OnExit();


        twist.SetActive(true);
        if (_movement._isFacingRight != twist.GetComponent<MonsterMovement>()._isFacingRight)
            twist.GetComponent<MonsterMovement>().Flip();

        twist.transform.position = transform.position;
        twist.GetComponent<Monster>().OnEnter();

        gameObject.SetActive(false);

    }


    public virtual void OnEnter()
    {

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