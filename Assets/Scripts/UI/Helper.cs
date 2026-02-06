using UnityEngine;

public class Helper : MonoBehaviour
{
    [TextArea(3, 10)] // 최소 3줄, 최대 10줄 크기
    public string _Description;
    public GameObject _gameObject;

    private GameObject _container;
    [HideInInspector] public bool _isEnable = false;

    [SerializeField] private Vector2 checkPivot = new Vector2(0f,0f); // 검사할 영역의 위치
    [SerializeField] private Vector2 checkPos = new Vector2(2.7f, 1.5f); // 검사할 영역의 위치
    [SerializeField] private Vector2 checkSize = new Vector2(2.7f, 2.7f); // 검사할 영역의 크기


    public void ToggleLore()
    {
        _isEnable = !_isEnable;

        if (_isEnable)
        {
            if (!IsOutsideScreen((Vector2)transform.position + checkPivot + new Vector2(checkPos.x * -1, checkPos.y)))
            {
                _container = Instantiate(_gameObject);
                _container.transform.position = (Vector2)transform.position + checkPivot + new Vector2(checkPos.x * -1, checkPos.y);
                _container.GetComponent<Lore>().SetText(_Description);
                return;
            }

            if (!IsOutsideScreen((Vector2)transform.position + checkPivot + new Vector2(checkPos.x * 1, checkPos.y)))
            {
                _container = Instantiate(_gameObject);
                _container.transform.position = (Vector2)transform.position + checkPivot + new Vector2(checkPos.x * 1, checkPos.y);
                _container.GetComponent<Lore>().SetText(_Description);
                return;
            }
        }
        else
        {
            Remove();
        }
    }

    public void Remove()
    {
        Destroy(_container);
    }



    private bool IsOutsideScreen(Vector3 pos)
    {
        // 1. 오브젝트의 월드 위치를 뷰포트 좌표로 변환
        Vector3 viewPos = Camera.main.WorldToViewportPoint(pos);

        // 2. x, y값이 0~1 사이라면 화면 안, 아니면 화면 밖
        if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)
        {
            return true; // 화면 밖임
        }

        return false; // 화면 안임
    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.magenta;
        Vector2 rightTarget = (Vector2)transform.position + checkPivot + new Vector2(checkPos.x * 1, checkPos.y);
        Vector2 leftTarget = (Vector2)transform.position + checkPivot + new Vector2(checkPos.x * -1, checkPos.y);


        Gizmos.DrawWireCube(rightTarget, checkSize);
        Gizmos.DrawWireCube(leftTarget, checkSize);
    }
}
