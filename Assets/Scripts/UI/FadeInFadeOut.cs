using System.Collections;
using System.Collections.Generic;
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
    private TextMeshPro _textMeshPro;

    private void Awake()
    {
        _image = fadeGameObject.GetComponent<Image>();
        _textMeshPro = fadeGameObject.GetComponent<TextMeshPro>();

        Color color = _image.color;
        color.a = 0f;
        _image.color = color;
        _textMeshPro.color = color;
    }

    public async UniTask StageClear()
    {
        await FadeInImage();

        await TextFadeIn();

        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        
        await TextFadeOut();

        await FadeOutImage();
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

        color.a = 0f;
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

        color.a = 1f;
        _textMeshPro.color = color;
    }
}
