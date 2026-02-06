using Cysharp.Threading.Tasks;
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
        LevelManager.Instance.LoadLevel(nextLevel);
    }
}
