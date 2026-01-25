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

    [Header("References")]
    public SpriteRenderer _WaterSprite;
    public SpriteRenderer _BubbleSprite;

    private float _currentBubbleHeight;
    private float _currentWaterHeight;
    private float _currentWidth;
    private BoxCollider2D _col;
    private float _currentY;

    // 제어 플래그
    private bool _isMoving = false;      // 현재 물리적으로 이동 중인가?
    private bool _shouldContinue = true; // 다음 칸으로 계속 갈 것인가?
    private Coroutine _activeRoutine;

    void Awake()
    {
        _currentWaterHeight = _WaterSprite.size.y;
        _currentWidth = _WaterSprite.size.x;
        _currentY = _WaterSprite.transform.position.y;
        _col = _WaterSprite.GetComponent<BoxCollider2D>();

        UpdateVisuals();

        StartRising();
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
            float currentTotal = _currentWaterHeight + _currentBubbleHeight;
            float targetTotal;

            if (isUpward)
                targetTotal = Mathf.Min(currentTotal + stepHeight, maxHeight);
            else
                targetTotal = Mathf.Max(currentTotal - stepHeight, 0);

            if (Mathf.Approximately(currentTotal, targetTotal)) break;

            // [한 칸 이동 시작]
            _isMoving = true;
            while (!Mathf.Approximately(_currentWaterHeight + _currentBubbleHeight, targetTotal))
            {
                float step = riseSpeed * Time.deltaTime;

                if (isUpward)
                {
                    if (_currentBubbleHeight < bubbleMaxHeight)
                        _currentBubbleHeight = Mathf.MoveTowards(_currentBubbleHeight, bubbleMaxHeight, step);
                    else
                        _currentWaterHeight = Mathf.MoveTowards(_currentWaterHeight, targetTotal - bubbleMaxHeight, step);
                }
                else
                {
                    if (_currentWaterHeight > 0)
                    {
                        float targetWater = Mathf.Max(0, targetTotal - _currentBubbleHeight);
                        _currentWaterHeight = Mathf.MoveTowards(_currentWaterHeight, targetWater, step);
                    }
                    else
                        _currentBubbleHeight = Mathf.MoveTowards(_currentBubbleHeight, 0, step);
                }

                UpdateVisuals();
                yield return null;
            }
            _isMoving = false;
            // [한 칸 이동 끝]

            // 다음 칸으로 가기 전 대기 (이 사이에 StopMovement가 호출되면 다음 루프에서 while문이 종료됨)
            yield return new WaitForSeconds(waitTime);

            if (isUpward && (_currentWaterHeight + _currentBubbleHeight) >= maxHeight) break;
            if (!isUpward && (_currentWaterHeight + _currentBubbleHeight) <= 0) break;
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
        _col.offset = new Vector2(0, totalHeight * 0.5f);
    }
}