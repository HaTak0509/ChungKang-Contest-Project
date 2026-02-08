using TMPro;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.UI;

public class FadeInFadeOut : MonoBehaviour
{
    public static FadeInFadeOut instance {get; private set;}

    [SerializeField] private GameObject fadeGameObject;
    [SerializeField] private GameObject textGameObject;

    private Image _image;
    private TextMeshProUGUI _textMeshPro;

    private void Awake()
    {
        instance = this;

        _image = fadeGameObject.GetComponent<Image>();
        _textMeshPro = textGameObject.GetComponent<TextMeshProUGUI>();

        SetAlpha(_image, 0f);
        SetAlpha(_textMeshPro, 0f);
    }

    private void SetAlpha(Image img, float a)
    {
        var c = img.color;
        c.a = a;
        img.color = c;
    }

    private void SetAlpha(TextMeshProUGUI tmp, float a)
    {
        var c = tmp.color;
        c.a = a;
        tmp.color = c;
    }

    public async UniTask StageClear(string stageName)
    {
        _textMeshPro.text = stageName;
        
        await FadeInImage();

        await TextFadeIn();

        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        
    }

    public async UniTask StageClear2() //이거 페이드인 페이드 아웃 문제로 분리함
    {
        await TextFadeOut();

        await FadeOutImage();

        _textMeshPro.text = "";
    }

    public async UniTask StageReset()
    {
        
        await FadeInImage();
        LevelManager.Instance.LoadLevel(LevelManager.Instance.currentLevelIndex);
        
        await FadeOutImage(); //이거 자연스러운 맵 전환을 위해 추가함

    }

    public void FadeIn()
    {
        FadeInImage().Forget();
    }

    public void FadeOut()
    {
        FadeOutImage().Forget();
    }

    private async UniTask FadeInImage()
    {
        float duration = 1f;
        float time = 0f;

        Color color = _image.color;

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, time / duration);

            color.a = alpha;
            _image.color = color;

            await UniTask.Yield();
        }

        color.a = 1f;
        _image.color = color;
    }


    private async UniTask FadeOutImage()
    {
        float duration = 1f;
        float time = 0f;

        Color color = _image.color;

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, time / duration);

            color.a = alpha;
            _image.color = color;

            await UniTask.Yield();
        }

        color.a = 0f;
        _image.color = color;
    }

    private async UniTask TextFadeIn()
    {
        float duration = 1f;
        float time = 0f;

        Color color = _textMeshPro.color;

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, time / duration);

            color.a = alpha;
            _textMeshPro.color = color;

            await UniTask.Yield();
        }

        color.a = 1f;
        _textMeshPro.color = color;
    }

    private async UniTask TextFadeOut()
    {
        float duration = 1f;
        float time = 0f;

        Color color = _textMeshPro.color;

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, time / duration);

            color.a = alpha;
            _textMeshPro.color = color;

            await UniTask.Yield();
        }

        color.a = 0f;
        _textMeshPro.color = color;
    }
}
