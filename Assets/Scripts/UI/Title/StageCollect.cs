using Cysharp.Threading.Tasks;
using UnityEngine;

public class StageCollect : MonoBehaviour
{
    [SerializeField] Animator _animator;
    public void CollectStage()
    {
        if (StageManager.Instance == null) return;

        PlayAndExecute("EndChapter").Forget();
    }


    public async UniTaskVoid PlayAndExecute(string animName)
    {
        _animator.Play(animName);

        await UniTask.Yield(PlayerLoopTiming.Update);

        var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        await UniTask.Delay(System.TimeSpan.FromSeconds(stateInfo.length));

        await FadeInFadeOut.instance.FadeInImage();

        LevelManager.Instance.LoadLevel(StageManager.Instance.currentLevel + 1);

        await FadeInFadeOut.instance.FadeOutImage();

    }

}
