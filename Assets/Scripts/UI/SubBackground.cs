using UnityEngine;

public class LongBackgroundRightStart : MonoBehaviour
{
    [Header("대상 설정")]
    [SerializeField] private Transform playerTransform;
    private RectTransform targetUI;

    [Header("패럴랙스 설정")]
    [Range(0.01f, 1.0f)]
    public float parallaxFactor = 0.2f;
    public float smoothTime = 0.15f;

    private Vector2 startAnchoredPos;
    private Vector2 currentVelocity;
    private float playerStartPosX;

    // 이동 가능한 범위
    private float minX;
    private float maxX;

    void Start()
    {
        targetUI = GetComponent<RectTransform>();

        // 1. 현재 우측 끝에 맞춰진 위치를 시작 위치로 저장
        startAnchoredPos = targetUI.anchoredPosition;

        if (playerTransform == null)
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        playerStartPosX = playerTransform.position.x;

        CalculateBoundaries();
    }

    void CalculateBoundaries()
    {
        float canvasWidth = transform.parent.GetComponent<RectTransform>().rect.width;
        float bgWidth = targetUI.rect.width;

        // 배경과 캔버스의 너비 차이 (배경이 움직일 수 있는 총 거리)
        float diff = bgWidth - canvasWidth;

        // 우측 끝에서 시작하므로, 현재 위치가 maxX(가장 오른쪽)가 됩니다.
        // 거기서 너비 차이만큼 뺀 곳이 minX(가장 왼쪽)가 됩니다.
        maxX = startAnchoredPos.x;
        minX = startAnchoredPos.x - diff;

        // 만약 피벗이 중앙(0.5)이고 우측 끝에 배치했다면 
        // 배경이 오른쪽으로 더 밀려나야 왼쪽 면이 보이므로 
        // 실제로는 더 큰 X값으로 이동하게 됩니다.
        // 상황에 따라 maxX와 minX의 방향을 자동 감지하도록 보정합니다.
        if (diff > 0)
        {
            // 우측 끝에 배치된 상태에서 왼쪽 면을 보려면 X값이 커져야 함 (오른쪽으로 이동)
            minX = startAnchoredPos.x;
            maxX = startAnchoredPos.x + diff;
        }
    }

    void Update()
    {
        if (targetUI == null || playerTransform == null) return;

        // 2. 플레이어의 이동량 (플레이어가 왼쪽으로 가면 -값이 나옴)
        float deltaX = playerTransform.position.x - playerStartPosX;

        // 3. 타겟 X 계산: 플레이어가 왼쪽(-)으로 가면 배경은 오른쪽(+)으로 이동
        // 100f는 월드 단위와 UI 단위의 차이를 메우기 위한 보정치입니다.
        float targetX = startAnchoredPos.x - (deltaX * parallaxFactor * 15f);

        // 4. 계산된 범위 내에서 제한
        float clampedX = Mathf.Clamp(targetX, minX, maxX);

        Vector2 targetPos = new Vector2(clampedX, startAnchoredPos.y);

        targetUI.anchoredPosition = Vector2.SmoothDamp(
            targetUI.anchoredPosition,
            targetPos,
            ref currentVelocity,
            smoothTime
        );
    }
}