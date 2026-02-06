using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class Elevator : MonoBehaviour, IInteractable
{
    [SerializeField] private int nextLevel;
    
    private FadeInFadeOut _fadeInFadeOut;
    private bool _action;

    private void Awake()
    {
        _fadeInFadeOut = FindAnyObjectByType<FadeInFadeOut>();
    }

    public void Interact()
    {
        if (!_action) return;

        RunStageClearAsync().Forget();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            _action = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            _action = false;
    }

    private async UniTask RunStageClearAsync()
    {
        await _fadeInFadeOut.StageClear($"Stage {nextLevel}");

        // 페이드 아웃의 마지막 color 설정이 적용될 수 있도록 여유
        await UniTask.NextFrame();       // 또는 await UniTask.DelayFrame(2);
        await UniTask.NextFrame();       // 2프레임 정도가 안전

        LevelManager.Instance.LoadLevel(nextLevel);
    }
}
