using UnityEngine;

public class TransparencyUI : MonoBehaviour
{
    public static TransparencyUI Instance { get; private set;}

    [SerializeField] private GameObject transparencyUI;

    public RectTransform trBar;
    public bool active;

    private float _currentMax;
    private float _MaxTrHeight;
    private SpriteRenderer _playerColor;


    private void Awake()
    {
        Instance = this;

        _playerColor = GetComponentInParent<SpriteRenderer>();

        _MaxTrHeight = trBar.sizeDelta.y;
        trBar.sizeDelta = new Vector2(trBar.sizeDelta.x, _MaxTrHeight);
        _currentMax = 1f;
    }

    private void Update()
    {
        if (!active)
        {
            transparencyUI.SetActive(false);
            return;
        }
        else
        {
            transparencyUI.SetActive(true);
        }

        if (_playerColor != null)
        {
            CheckTransparencyBar();
        }
    }

    public void CheckTransparencyBar()
    {
        float uiAlpha = Mathf.InverseLerp(0.1f, 1f, _playerColor.color.a);

        // alpha에 따라 최대값이 줄어들게
        _currentMax = Mathf.Clamp01(uiAlpha);

        // 실제 UI 높이 계산
        float height = Mathf.Lerp(trBar.sizeDelta.y, _MaxTrHeight * _currentMax, Time.deltaTime);
        trBar.sizeDelta = new Vector2(trBar.sizeDelta.x, height);
    }
}
