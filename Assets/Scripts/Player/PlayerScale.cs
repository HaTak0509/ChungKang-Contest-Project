using UnityEngine;

public class PlayerScale : MonoBehaviour
{

    [Header("디버깅용 표시")]
    [SerializeField] float _Scale = 100;
    float _MaxScale = 100;
    public bool _isDown = false;

    [Header("최소 크기")] [Range(0f,100f)] public float _MinScale = 20;



    public static PlayerScale Instance;
    Vector3 _OriginScale;

    // Start is called before the first frame update
    void Start()
    {
        _OriginScale = transform.localScale;
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isDown && _Scale < _MaxScale)
        {
            // 현재 크기의 일정 비율만큼 증가
            float amount = _Scale * 0.25f * Time.deltaTime;
            AddScale(amount);
        }
        else if (_isDown && _Scale > _MinScale)
        {
            // 현재 크기의 일정 비율만큼 감소 (이것이 유저님이 원하신 '일정 비율 감소')
            float amount = _Scale * 0.5f * Time.deltaTime;
            AddScale(-amount);
        }

        if (_Scale < _MaxScale)
        {
            ScaleUI.Instance._ScaleUI.SetActive(true);
        }
        else
        {
            ScaleUI.Instance._ScaleUI.SetActive(false);
        }

        transform.localScale = _OriginScale * _Scale / 100f;
        ScaleUI.Instance.CheckScaleBar(_Scale, _MaxScale);
    }


    public void AddScale(float AddScale)
    {
        _Scale += AddScale;
    }
}