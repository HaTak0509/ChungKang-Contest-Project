using UnityEngine;

public class Box : MonoBehaviour , IInteractable, WarpingInterface
{
    [Header("검사 설정")]
    public GameObject _Box;
    [SerializeField] private Vector2 checkSize = new Vector2(1.5f, 3.0f); // 검사할 영역의 크기
    [SerializeField] private float checkDistance = 1.6f;              // 좌우로 얼마나 떨어진 곳을 검사할지
    private PushingObject _pushingObject;

    private bool _isTwist = false;
    public bool isTwist => _isTwist;
    private Transform _Player;

    private void Start()
    {
        _pushingObject = GetComponent<PushingObject>();
        _Player = GameObject.FindWithTag("Player").transform;

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
            if(_Box.GetComponent<Box>().isTwist)
                RelocateToEmptySpace();
    
        }else if(PlayerScale.Instance._Scale >= 100)
        {
            _pushingObject.Interact();
        }

    }

    public void RelocateToEmptySpace()
    {
        // 1. 오른쪽 확인
        Vector2 rightTarget = (Vector2)transform.position + Vector2.right * checkDistance;
        if (!IsObstacleAt(rightTarget))
        {
            MoveTo(rightTarget);
            return;
        }

        // 2. 왼쪽 확인
        Vector2 leftTarget = (Vector2)transform.position + Vector2.left * checkDistance;
        if (!IsObstacleAt(leftTarget))
        {
            MoveTo(leftTarget);
            return;
        }

        Debug.Log("좌우 모두 비어있지 않습니다!");
    }

    // 해당 위치에 장애물이 있는지 체크
    private bool IsObstacleAt(Vector2 targetPos)
    {
        // 지정된 위치에 checkSize만큼의 박스를 그려 충돌체가 있는지 확인
        Collider2D hit = Physics2D.OverlapBox(targetPos, checkSize, 0f);
        return hit != null;
    }

    private void MoveTo(Vector2 newPos)
    {
        _Player.position = new Vector3(newPos.x, newPos.y + checkSize.y / 2, _Player.position.z);
    }

    // 에디터 뷰에서 검사 영역 시각화
    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.green;
        Vector2 rightTarget = (Vector2)_Box.transform.position + Vector2.right * checkDistance;
        Vector2 leftTarget = (Vector2)_Box.transform.position + Vector2.left * checkDistance;


        Gizmos.DrawWireCube(rightTarget, checkSize);
        Gizmos.DrawWireCube(leftTarget, checkSize);
    }


}
