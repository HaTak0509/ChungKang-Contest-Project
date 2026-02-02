using UnityEngine;
using System.Collections;

public class WaterRiseController : MonoBehaviour
{
    [Header("Water Settings")]
    [SerializeField] private float riseSpeed = 8.0f;
    [SerializeField] private float stepHeight = 1.0f;
    [SerializeField] private float waitTime = 1.0f;
    [SerializeField] private float maxHeight = 15f;
    [SerializeField] private float bubbleMaxHeight = 1f;


    [SerializeField] private float WaterSize; 

    [Header("References")]
    public SpriteRenderer _WaterSprite;
    public SpriteRenderer _BubbleSprite;

    [Header("Status")]
    [SerializeField] private int _currentStep = 0; // 현재 물의 단계 (0부터 시작)
    public int CurrentStep => _currentStep; // 외부에서 읽기 전용으로 가져갈 수 있는 프로퍼티v


    public static WaterRiseController Instance;
    private float _currentBubbleHeight;
    private float _currentWaterHeight;
    private float _currentWidth;
    private BoxCollider2D _col;
    private float _currentY;

    // 제어 플래그
    private bool _shouldContinue = true; // 다음 칸으로 계속 갈 것인가?
    private Coroutine _activeRoutine;

    void Awake()
    {
        
        _currentWaterHeight = _WaterSprite.size.y;

        _WaterSprite.size = new Vector2(WaterSize, _WaterSprite.size.y);

        _currentWidth = _WaterSprite.size.x;
        _currentY = _WaterSprite.transform.position.y;
        _col = _WaterSprite.GetComponent<BoxCollider2D>();
        Instance = this;



        UpdateVisuals();
    }

    // --- 제어 함수 ---

    public void StartRising()
    {
        _shouldContinue = true;
        if (_activeRoutine != null) StopCoroutine(_activeRoutine);
        _activeRoutine = StartCoroutine(HandleWaterMovement(true));
    }

    public void StartFalling()
    {
        _shouldContinue = true;
        if (_activeRoutine != null) StopCoroutine(_activeRoutine);
        _activeRoutine = StartCoroutine(HandleWaterMovement(false));
    }

    public void StopMovement()
    {
        // 다음 칸으로 넘어가지 않도록 설정 (현재 이동중인 칸은 마저 이동함)
        _shouldContinue = false;
    }

    // --- 핵심 로직 ---

    private IEnumerator HandleWaterMovement(bool isUpward)
    {
        while (_shouldContinue)
        {
            // 1. 목표 단계(Step) 결정
            int targetStep;
            if (isUpward)
                targetStep = _currentStep + 1; // 무조건 다음 단계로
            else
                targetStep = _currentStep - 1; // 무조건 이전 단계로

            // 2. 최대/최소 단계 제한
            int maxStepCount = Mathf.FloorToInt(maxHeight / stepHeight);
            targetStep = Mathf.Clamp(targetStep, 0, maxStepCount);

            // 3. 목표 단계를 바탕으로 실제 목표 높이(targetTotal) 계산
            float targetTotal = targetStep * stepHeight;

            // 이미 목표 단계라면 종료
            if (Mathf.Abs((_currentWaterHeight + _currentBubbleHeight) - targetTotal) < 0.001f) break;

            // [한 칸 이동 시작]
            while (Mathf.Abs((_currentWaterHeight + _currentBubbleHeight) - targetTotal) > 0.001f)
            {
                float step = riseSpeed * Time.deltaTime;

                if (isUpward)
                {
                    // 상승 로직 (목표: targetTotal)
                    if (_currentBubbleHeight < bubbleMaxHeight)
                        _currentBubbleHeight = Mathf.MoveTowards(_currentBubbleHeight, bubbleMaxHeight, step);
                    else
                        _currentWaterHeight = Mathf.MoveTowards(_currentWaterHeight, targetTotal - bubbleMaxHeight, step);
                }
                else
                {
                    // 하강 로직 (목표: targetTotal)
                    if (_currentWaterHeight > 0.001f)
                    {
                        // 물부터 줄임 (목표 물 높이 = 목표 전체 높이 - 거품 높이)
                        float targetWater = Mathf.Max(0, targetTotal - _currentBubbleHeight);
                        _currentWaterHeight = Mathf.MoveTowards(_currentWaterHeight, targetWater, step);
                    }
                    else
                    {
                        // 그 다음 거품 줄임
                        _currentBubbleHeight = Mathf.MoveTowards(_currentBubbleHeight, targetTotal, step);
                    }
                }

                UpdateVisuals();
                yield return null;
            }
            // [한 칸 이동 끝]

            // 4. 이동 완료 후 현재 단계 갱신
            _currentStep = targetStep;

            Debug.Log($"이동 완료! 현재 단계: {_currentStep}, 높이: {targetTotal}");

            yield return new WaitForSeconds(waitTime);

            // 끝에 도달하면 루프 종료
            if (isUpward && _currentStep >= maxStepCount) break;
            if (!isUpward && _currentStep <= 0) break;
        }
        _activeRoutine = null;
    }
    void UpdateVisuals()
    {
        _WaterSprite.size = new Vector2(_currentWidth, _currentWaterHeight);
        _WaterSprite.transform.position = new Vector3(
            _WaterSprite.transform.position.x,
            _currentY + _currentWaterHeight * 0.5f,
            _WaterSprite.transform.position.z
        );

        _BubbleSprite.size = new Vector2(_currentWidth, _currentBubbleHeight);
        _BubbleSprite.transform.position = new Vector3(
            _WaterSprite.transform.position.x,
            _currentY + _currentWaterHeight + _currentBubbleHeight * 0.5f,
            _WaterSprite.transform.position.z
        );

        float totalHeight = _currentWaterHeight + _currentBubbleHeight;
        _col.size = new Vector2(_currentWidth, totalHeight);
        _col.offset = new Vector2(0, 0.5f );
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + new Vector3(0, maxHeight / 2, 0), new Vector2(WaterSize, maxHeight));
    }
}