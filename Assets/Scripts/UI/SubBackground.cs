using UnityEngine;

public class LongBackgroundUIPlayer : MonoBehaviour
{
    [Header("대상 설정")]
    [SerializeField] private RectTransform playerRect; // 이제 Transform이 아닌 RectTransform입니다.
    private RectTransform targetUI;

    [Header("패럴랙스 설정")]
    [Range(0.01f, 1.0f)]
    public float parallaxFactor = 0.2f;
    public float smoothTime = 0.15f;

    private Vector2 startAnchoredPos;
    private Vector2 currentVelocity;
    private float playerStartPosX;

    private float minX;
    private float maxX;

    void Start()
    {
        targetUI = GetComponent<RectTransform>();

        // 1. 초기 위치 저장
        startAnchoredPos = targetUI.anchoredPosition;

        if (playerRect == null)
        {
            // 태그로 찾을 경우 RectTransform으로 가져옵니다.
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) playerRect = playerObj.GetComponent<RectTransform>();
        }

        if (playerRect != null)
        {
            // 플레이어의 anchoredPosition.x를 시작점으로 저장
            playerStartPosX = playerRect.anchoredPosition.x;
        }

        CalculateBoundaries();
    }

    void CalculateBoundaries()
    {
        // 부모(Canvas) 너비와 배경 너비 차이 계산
        RectTransform parentRect = transform.parent.GetComponent<RectTransform>();
        float canvasWidth = parentRect.rect.width;
        float bgWidth = targetUI.rect.width;

        float diff = bgWidth - canvasWidth;

        // 우측 끝 시작 기준: 플레이어가 왼쪽으로 갈 때 배경이 오른쪽으로 밀리는 범위
        if (diff > 0)
        {
            minX = startAnchoredPos.x;
            maxX = startAnchoredPos.x + diff;
        }
        else
        {
            minX = maxX = startAnchoredPos.x;
        }
    }

    void Update()
    {
        if (targetUI == null || playerRect == null) return;

        // 2. 플레이어의 이동량 (픽셀 단위로 직접 계산)
        // 플레이어와 배경이 같은 Canvas 공간에 있으므로 별도의 단위 변환(100f 등)이 필요 없습니다.
        float deltaX = playerRect.anchoredPosition.x - playerStartPosX;

        // 3. 타겟 X 계산
        // 플레이어 이동량(픽셀) * 패럴랙스 계수
        float targetX = startAnchoredPos.x - (deltaX * parallaxFactor);

        // 4. 범위 제한
        float clampedX = Mathf.Clamp(targetX, minX, maxX);

        Vector2 targetPos = new Vector2(clampedX, startAnchoredPos.y);

        // 5. 부드러운 이동
        targetUI.anchoredPosition = Vector2.SmoothDamp(
            targetUI.anchoredPosition,
            targetPos,
            ref currentVelocity,
            smoothTime
        );
    }

    // 해상도 변경 대응
    private void OnRectTransformDimensionsChange()
    {
        if (targetUI != null) CalculateBoundaries();
    }
}