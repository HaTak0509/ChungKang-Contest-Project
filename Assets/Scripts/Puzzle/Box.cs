using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class Box : MonoBehaviour , IInteractable, WarpingInterface
{
    [Header("박스 설정")]
    public GameObject _BoxTeleport;
    [SerializeField] private LayerMask _LayerMask;
    [SerializeField] private Vector2 checkPos = new Vector2(0.5f, 0.0f); // 검사할 영역의 위치
    [SerializeField] private Vector2 checkSize = new Vector2(1.5f, 3.0f); // 검사할 영역의 크기
    [SerializeField] private float _Cooltime = 0.3f;
    [SerializeField] private float _BoxOpenTime = 0.7f;


    [Header("필수 연결")]
    [SerializeField] private SpriteRenderer _SpriteRenderer;
    [SerializeField] private Sprite _Open;
    [SerializeField] private Sprite _Close;


    private PushingObject _pushingObject;
    private bool _isCool = false;

    private bool _isTwist = false;
    public bool isTwist => _isTwist;
    private Transform _Player;

    private void Start()
    {
        _pushingObject = GetComponent<PushingObject>();
        _Player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if(_isTwist && PlayerScale.Instance._Scale <= 30 && _BoxTeleport.GetComponent<Box>().isTwist)
        {
            _SpriteRenderer.sprite = _Open;
        }
        else
        {
            _SpriteRenderer.sprite = _Close;
        }

    }

    public void Warping()
    {
        _isTwist = !_isTwist;
    }

    public void Interact()
    {
        if (!_isTwist)
        {
            _pushingObject.Interact();
            return;
        }



        if(PlayerScale.Instance._Scale <= 30 )
        {
            if (_BoxTeleport.GetComponent<Box>().isTwist && !_isCool)
                RelocateToEmptySpace();
            else
                Debug.Log("야 이거, 안 뒤틀렸는데?");
    
        }else if(PlayerScale.Instance._Scale >= 100)
        {
            _pushingObject.Interact();
        }

    }

    public void RelocateToEmptySpace()
    {
        StartFlash();
        // 1. 오른쪽 확인
        Vector2 rightTarget = (Vector2)_BoxTeleport.transform.position + new Vector2(checkPos.x * 1,checkPos.y);
        if (!IsObstacleAt(rightTarget))
        {
            MoveTo(rightTarget);
            return;
        }
        else
        {
            Debug.Log("오른쪽 자리 없음");
        }

        // 2. 왼쪽 확인
        Vector2 leftTarget = (Vector2)_BoxTeleport.transform.position + new Vector2(checkPos.x * -1, checkPos.y);
        if (!IsObstacleAt(leftTarget))
        {
            MoveTo(leftTarget);
            return;
        }
        else
        {
            Debug.Log("왼쪽없음ㅡㅡ");
        }

        Debug.Log("좌우 모두 비어있지 않습니다!");
    }

    // 해당 위치에 장애물이 있는지 체크
    private bool IsObstacleAt(Vector2 targetPos)
    {
        // 지정된 위치에 checkSize만큼의 박스를 그려 충돌체가 있는지 확인
        Collider2D hit = Physics2D.OverlapBox(targetPos, checkSize, 0f,_LayerMask);
        return hit != null;
    }

    private void MoveTo(Vector2 newPos)
    {
        _Player.position = new Vector3(newPos.x, newPos.y + checkSize.y / 2, _Player.position.z);
        _BoxTeleport.GetComponent<Box>().StartFlash();
    }

    public void StartFlash()
    {
        // UniTask는 Forget()을 호출하여 비동기로 실행하거나, 
        // 다른 async 함수 내에서 await으로 기다릴 수 있습니다.
        SetCoolTime().Forget();
        
    }

    private async UniTask SetCoolTime()
    {
        _isCool = true;

        // 오브젝트가 파괴되면 대기를 즉시 중단함 (에러 방지)
        await UniTask.Delay(TimeSpan.FromSeconds(_Cooltime), cancellationToken: this.GetCancellationTokenOnDestroy());

        _isCool = false;
    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.green;
        Vector2 rightTarget = (Vector2)_BoxTeleport.transform.position + new Vector2(checkPos.x * 1, checkPos.y);
        Vector2 leftTarget = (Vector2)_BoxTeleport.transform.position + new Vector2(checkPos.x * -1, checkPos.y);


        Gizmos.DrawWireCube(rightTarget, checkSize);
        Gizmos.DrawWireCube(leftTarget, checkSize);
    }


}
