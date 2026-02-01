using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("추적 설정")]
    [SerializeField] private Transform target;      // 따라다닐 플레이어
    [SerializeField] private float smoothing = 5f;  // 추적 부드러움 정도
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10); // 카메라 깊이 유지

    [Header("구역 제한 (Confiner)")]
    [SerializeField] private Vector2 minBoundary;   // 카메라가 갈 수 있는 최소 X, Y
    [SerializeField] private Vector2 maxBoundary;   // 카메라가 갈 수 있는 최대 X, Y

    [Header("줌(Zoom) 설정")]
    [SerializeField] private float normalSize = 5f; // 플레이어 100일 때 크기
    [SerializeField] private float zoomedSize = 3f; // 플레이어 20일 때 크기
    [SerializeField] private float zoomSpeed = 5f;

    [Header("전체 보기 모드 설정")]
    [SerializeField] private float overviewSmoothSpeed = 2f; // 전체 보기 전환 시 속도
    private bool _isOverviewMode = false; // R키 토글 상태

    private Camera _cam;

    void Start()
    {
        _cam = GetComponent<Camera>();

        // 타겟이 설정 안 되어 있다면 인스턴스로 찾기
        if (target == null && PlayerScale.Instance != null)
            target = PlayerScale.Instance.transform;
    }
    void Update()
    {
        // R키를 누르면 모드 토글
        if (Input.GetKeyDown(KeyCode.R))
        {
            _isOverviewMode = !_isOverviewMode;
            Debug.Log(_isOverviewMode ? "전체 보기 모드" : "추적 모드");
        }
    }
    // 카메라 이동은 모든 Update가 끝난 뒤 LateUpdate에서 처리하는 것이 떨림 방지에 좋습니다.
    void LateUpdate()
    {
        if (target == null) return;

        if (_isOverviewMode)
        {
            ShowFullBoundary();
        }
        else
        {
            HandleMovement();
            HandleZoom();
        }
    }

    private void HandleMovement()
    {
        // 1. 목표 위치 계산
        Vector3 targetPos = target.position + offset;

        // 2. 구역 제한 (Clamp) 적용
        // 카메라의 절반 크기(OrthographicSize)를 고려해야 화면 끝이 구역 밖으로 안 나갑니다.
        float camHeight = _cam.orthographicSize;
        float camWidth = camHeight * _cam.aspect;

        float clampedX = Mathf.Clamp(targetPos.x, minBoundary.x + camWidth, maxBoundary.x - camWidth);
        float clampedY = Mathf.Clamp(targetPos.y, minBoundary.y + camHeight, maxBoundary.y - camHeight);

        Vector3 clampedPos = new Vector3(clampedX, clampedY, targetPos.z);

        // 3. 부드러운 이동 (Lerp)
        transform.position = Vector3.Lerp(transform.position, clampedPos, Time.deltaTime * smoothing);
    }

    private void HandleZoom()
    {
        // PlayerScale 스크립트와 연동 (100 -> 1.0, 20 -> 0.0)
        float t = (PlayerScale.Instance._Scale - 20f) / (100f - 20f);

        // 비율에 따라 줌 크기 결정
        float targetSize = Mathf.Lerp(zoomedSize, normalSize, t);

        // 부드럽게 카메라 렌즈 크기 변경
        _cam.orthographicSize = Mathf.Lerp(_cam.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);
    }
    private void ShowFullBoundary()
    {
        // 1. 구역의 중심점 계산
        float centerX = (minBoundary.x + maxBoundary.x) / 2f;
        float centerY = (minBoundary.y + maxBoundary.y) / 2f;
        Vector3 centerPos = new Vector3(centerX, centerY, offset.z);

        // 2. 전체 구역을 다 담기 위한 Orthographic Size 계산
        float boundaryHeight = (maxBoundary.y - minBoundary.y) / 2f;
        float boundaryWidth = (maxBoundary.x - minBoundary.x) / 2f;

        // 화면 가로세로비(Aspect)를 고려하여 폭과 높이 중 더 큰 쪽을 기준으로 사이즈 결정
        float requiredSizeByWidth = boundaryWidth / _cam.aspect;
        float targetOverviewSize = Mathf.Max(boundaryHeight, requiredSizeByWidth);

        // 3. 부드럽게 이동 및 줌 적용
        transform.position = Vector3.Lerp(transform.position, centerPos, Time.deltaTime * overviewSmoothSpeed);
        _cam.orthographicSize = Mathf.Lerp(_cam.orthographicSize, targetOverviewSize, Time.deltaTime * overviewSmoothSpeed);
    }
    // 에디터 뷰에서 제한 구역을 시각적으로 확인하기 위한 기능
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 center = new Vector3((minBoundary.x + maxBoundary.x) / 2, (minBoundary.y + maxBoundary.y) / 2, 0);
        Vector3 size = new Vector3(maxBoundary.x - minBoundary.x, maxBoundary.y - minBoundary.y, 1);
        Gizmos.DrawWireCube(center, size);
    }
}