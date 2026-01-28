using UnityEngine;
using System.Collections;
using TMPro;
using System.Runtime.InteropServices;

public class ScaleUI : MonoBehaviour
{
    [Header("Scale_ UI")]
    public RectTransform _ScaleBar;
    public GameObject _ScaleUI;

    public static ScaleUI Instance;
    private Coroutine _ScaleBarRoutine;
    private float _MaxScaleHeight;

    private Vector3 _parentScale;

    private void Start()
    {
        _MaxScaleHeight = _ScaleBar.sizeDelta.y;
        Instance = this;
        _parentScale = transform.parent.lossyScale;
    }

    private void Update()
    {
        if (transform.parent != null)
        {
            transform.localScale = new Vector3(
                _parentScale.x / transform.parent.lossyScale.x,
                _parentScale.y / transform.parent.lossyScale.y,
                _parentScale.z / transform.parent.lossyScale.z
            );
        }
    }

    public void CheckScaleBar(float Scale = 0, float MaxScale = 0)
    {
        float percent = Scale / MaxScale;
        float targetHeight = _MaxScaleHeight * percent;


        if (_ScaleBarRoutine != null)
            StopCoroutine(_ScaleBarRoutine);

        _ScaleBarRoutine = StartCoroutine(SmoothChangeBar(_ScaleBar, targetHeight));
    
    }
    private IEnumerator SmoothChangeBar(RectTransform Bar,float targetHeight)
    {
        float duration = 0.05f;
        float elapsed = 0f;
        float startHeight = Bar.sizeDelta.y;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            
            float smooth = Mathf.SmoothStep(startHeight, targetHeight, t);//부드러운 보간

            Bar.sizeDelta = new Vector2(Bar.sizeDelta.x, smooth);
            yield return null;
        }

        Bar.sizeDelta = new Vector2(Bar.sizeDelta.x, targetHeight); //정확히 맞추기
        _ScaleBarRoutine = null;
    }
}
