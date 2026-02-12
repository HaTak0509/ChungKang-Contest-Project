using UnityEngine;

public class WaterUI : MonoBehaviour
{
    public static WaterUI Instance { get; private set;}

    [SerializeField] private GameObject transparencyUI;
    [SerializeField] private Damageable damageable;

    public RectTransform trBar;
    public bool active;

    private float _currentMax;
    private float _MaxTrHeight;
    public float _currentWater;

    private void Awake()
    {
        Instance = this;


        _MaxTrHeight = trBar.sizeDelta.y;
        trBar.sizeDelta = new Vector2(trBar.sizeDelta.x, _MaxTrHeight);
        _currentMax = 1f;
    }

    private void Update()
    {
        if (_currentWater <= 0.0f)
        {
            damageable.GameOver();
        }

        if (!active && _currentWater == 1f)
        {
            transparencyUI.SetActive(false);
            return;
        }
        
        if (active)
        {
            transparencyUI.SetActive(true);
            CheckTransparencyBar();

        }
    }

    public void CheckTransparencyBar()
    {
        float uiAlpha = Mathf.InverseLerp(0f, 1f, _currentWater);

        // alpha에 따라 최대값이 줄어들게
        _currentMax = Mathf.Clamp01(uiAlpha);

        // 실제 UI 높이 계산
        float height = Mathf.Lerp(trBar.sizeDelta.y, _MaxTrHeight * _currentMax, Time.deltaTime * 20);
        trBar.sizeDelta = new Vector2(trBar.sizeDelta.x, height);
    }
}
