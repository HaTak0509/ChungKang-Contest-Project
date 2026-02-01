using System.Collections;
using UnityEngine;

public class PlayerScale : MonoBehaviour
{

    public float _Scale { get; private set; } = 100;
    private float _MaxScale = 100;

    [Header("최소 크기")] [Range(0f,100f)] public float _MinScale = 20;

    private Coroutine _scaleRoutine;

    public static PlayerScale Instance;
    Vector3 _OriginScale;

    void Start()
    {
        _Scale = _MaxScale;

        _OriginScale = transform.localScale;
        Instance = this;
    }

    public void TriggerScaleChange()
    {
        if (_scaleRoutine != null) StopCoroutine(_scaleRoutine);

        // 100이면 20으로, 20이면 100으로 목표 설정
        float target = (_Scale > _MinScale) ? _MinScale : _MaxScale;
        

        _scaleRoutine = StartCoroutine(ScaleLerp(target));
    }

    IEnumerator ScaleLerp(float target)
    {
        float duration = 2.5f; // 0.2초 만에 빠르게 변화
        float elapsed = 0f;
        float startScale = _Scale;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // 부드러운 보간법 적용
            _Scale = Mathf.Lerp(startScale, target, t);

            // 실제 스케일 반영
            float temp = _Scale / _MaxScale;
            transform.localScale = _OriginScale * temp;
            //ScaleUI.Instance.CheckScaleBar(_Scale, _MaxScale);

            yield return null;
        }

        _Scale = target;
    }
    
}