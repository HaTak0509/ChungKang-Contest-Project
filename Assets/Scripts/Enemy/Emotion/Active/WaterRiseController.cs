using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class WaterRiseController : MonoBehaviour
{

    enum WaterRisePhase
    {
        BubbleOnly,   // 거품만 자람
        WaterRising   // 물이 자람
    }

    [Header("Water Settings")]
    [SerializeField] private float riseSpeed = 0.5f; // 초당 상승 높이
    [SerializeField] private float maxHeight = 15f;
    [SerializeField] private float bubbleMaxHeight = 1f;


    public SpriteRenderer _WaterSprite;
    public SpriteRenderer _BubbleSprite;



    private WaterRisePhase _phase = WaterRisePhase.BubbleOnly;
    private float _currentBubbleHeight;


    private float _currentHeight;
    private float _currentWidth;
    private BoxCollider2D _col;

    private float _currentY;

    void Awake()
    {
        
        _currentHeight = _WaterSprite.size.y;
        _currentWidth = _WaterSprite.size.x;
        _currentY = _WaterSprite.transform.position.y;

        _col = _WaterSprite.GetComponent<BoxCollider2D>();


        StartRise();
    }

    void Update()
    {
      

       
    }

    void SetHeight(float height)
    {
        _WaterSprite.size = new Vector2(_currentWidth, height);

        _WaterSprite.transform.position =
            new Vector3(
                _WaterSprite.transform.position.x,
                _currentY + height * 0.5f,
                _WaterSprite.transform.position.z
            );

        _col.size = new Vector2(_currentWidth, height);
    }

    public void StartRise()
    {
        StartCoroutine(WaterUpCoroutine(riseSpeed));
    }


    private IEnumerator WaterUpCoroutine(float riseSpeed)
    {
        while (true)
        {
            switch (_phase)
            {
                case WaterRisePhase.BubbleOnly:
                    UpdateBubbleOnly();
                    break;

                case WaterRisePhase.WaterRising:
                    UpdateWaterRising(riseSpeed);
                    break;
            }

            if (_currentHeight >= maxHeight)
                break;

            yield return null;
        }
    }

    void UpdateBubbleOnly()
    {
        _currentBubbleHeight += riseSpeed * Time.deltaTime;
        _currentBubbleHeight = Mathf.Min(_currentBubbleHeight, bubbleMaxHeight);

        // 거품만 길어짐
        _BubbleSprite.size = new Vector2(_currentWidth, _currentBubbleHeight);

        // 위치는 물 위에서 고정
        _BubbleSprite.transform.position =
            new Vector3(
                _WaterSprite.transform.position.x,
                _currentY + _currentHeight + _currentBubbleHeight * 0.5f,
                _WaterSprite.transform.position.z
            );

        // 임계치 도달 → 물 상승 단계로 전환
        if (_currentBubbleHeight >= bubbleMaxHeight)
        {
            _phase = WaterRisePhase.WaterRising;
        }
    }


    void UpdateWaterRising(float riseSpeed)
    {
        _currentHeight += riseSpeed * Time.deltaTime;
        SetHeight(_currentHeight);

        // 거품은 물 위에 얹힘 (길이는 고정)
        _BubbleSprite.transform.position =
            new Vector3(
                _WaterSprite.transform.position.x,
                _currentY + _currentHeight + _currentBubbleHeight * 0.5f,
                _WaterSprite.transform.position.z
            );
    }

}
