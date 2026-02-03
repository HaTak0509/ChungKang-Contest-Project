using UnityEngine;
using System.Collections;

public class WaterRiseController : MonoBehaviour
{
    [Header("Water Settings (Scale Mode)")]
    [SerializeField] private float riseSpeed = 8.0f;
    [SerializeField] private float stepHeight = 1.0f;
    [SerializeField] private float waitTime = 1.0f;

    [SerializeField] private Vector2 WaterSize = new Vector2(20,10);

    [Header("References")]
    public Transform _WaterTransform;      // 물: Scale을 수정 (Sprite Mask용)
    public SpriteRenderer _BubbleSprite;   // 거품: Size를 수정

    [Header("Status")]
    [SerializeField] private int _currentStep = 0;
    public int CurrentStep => _currentStep;

    public static WaterRiseController Instance;

    private float _currentWaterHeight = 0f;
    private float _currentBubbleHeight = 0f;
    private bool _shouldContinue = true;
    private Coroutine _activeRoutine;

    void Awake()
    {
        Instance = this;

        // 초기 사이즈 세팅
        UpdateVisuals();
    }

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

    private IEnumerator HandleWaterMovement(bool isUpward)
    {
        while (_shouldContinue)
        {
            int targetStep = isUpward ? _currentStep + 1 : _currentStep - 1;
            int maxStepCount = Mathf.FloorToInt(WaterSize.y / stepHeight);
            targetStep = Mathf.Clamp(targetStep, 0, maxStepCount);

            float targetTotal = targetStep * stepHeight;

            while (Mathf.Abs((_currentWaterHeight + _currentBubbleHeight) - targetTotal) > 0.001f)
            {
                float step = riseSpeed * Time.deltaTime;

                if (isUpward)
                {
                    // 1. 거품부터 생성 (거품 높이가 최대치보다 낮으면 거품부터 키움)
                    if (_currentBubbleHeight < 1)
                        _currentBubbleHeight = Mathf.MoveTowards(_currentBubbleHeight, 1, step);
                    // 2. 거품이 다 찼으면 물(Scale)을 높임
                    else
                        _currentWaterHeight = Mathf.MoveTowards(_currentWaterHeight, targetTotal - 1, step);
                }
                else
                {
                    // 하강할 때는 물(Scale)부터 줄임
                    if (_currentWaterHeight > 0.001f)
                    {
                        float targetWater = Mathf.Max(0, targetTotal - _currentBubbleHeight);
                        _currentWaterHeight = Mathf.MoveTowards(_currentWaterHeight, targetWater, step);
                    }
                    // 그 다음 거품 높이를 줄임
                    else
                    {
                        _currentBubbleHeight = Mathf.MoveTowards(_currentBubbleHeight, targetTotal, step);
                    }
                }

                UpdateVisuals();
                yield return null;
            }

            _currentStep = targetStep;
            yield return new WaitForSeconds(waitTime);

            if (isUpward && _currentStep >= maxStepCount) break;
            if (!isUpward && _currentStep <= 0) break;
        }
        _activeRoutine = null;
    }

    void UpdateVisuals()
    {
        // 1. 물 (Scale 수정)
        // 일반적으로 Center인 경우를 대비해 위치도 함께 계산합니다.
        _WaterTransform.localScale = new Vector3(WaterSize.x, _currentWaterHeight, 1);
        _WaterTransform.localPosition = new Vector3(0, _currentWaterHeight * 0.5f, 0);

        // 2. 거품 (SpriteRenderer Size 수정)
        _BubbleSprite.size = new Vector2(WaterSize.x, _currentBubbleHeight);
        // 거품의 위치는 항상 물의 머리 꼭대기
        _BubbleSprite.transform.localPosition = new Vector3(0, _currentWaterHeight + (_currentBubbleHeight * 0.25f), 0);

        // 3. 콜라이더 (전체 높이 반영)
        float totalHeight = _currentWaterHeight + _currentBubbleHeight;
    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.blue;
 
        Gizmos.DrawWireCube(transform.position + new Vector3(0,WaterSize.y / 2), new Vector2(WaterSize.x,WaterSize.y - 1));
    }
}