using UnityEngine;

public class CameraZoneTrigger : MonoBehaviour
{
    private BoxCollider2D _zoneCollider;
    private CameraController _camCtrl;

    [SerializeField] private Vector2 minBoundary;   // 카메라가 갈 수 있는 최소 X, Y
    [SerializeField] private Vector2 maxBoundary;   // 카메라가 갈 수 있는 최대 X, Y

    void Awake()
    {
        _zoneCollider = GetComponent<BoxCollider2D>();
        _camCtrl = Camera.main.GetComponent<CameraController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 콜라이더의 영역만큼 카메라 제한 구역을 변경
            _camCtrl.SetBoundary(new Vector2(minBoundary.x, minBoundary.y),
                                 new Vector2(maxBoundary.x, maxBoundary.y));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 구역을 나가면 다시 전체 맵 경계로 복구
            _camCtrl.ResetBoundary();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 center = new Vector3((minBoundary.x + maxBoundary.x) / 2, (minBoundary.y + maxBoundary.y) / 2, 0);
        Vector3 size = new Vector3(maxBoundary.x - minBoundary.x, maxBoundary.y - minBoundary.y, 1);
        Gizmos.DrawWireCube(center, size);
    }

}