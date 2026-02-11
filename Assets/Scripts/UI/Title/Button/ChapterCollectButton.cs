using Cysharp.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class ChapterCollectButton : MonoBehaviour
{
    [SerializeField] GameObject title;
    [SerializeField] GameObject chaptercollect;
    [SerializeField] GameObject exit;
    [SerializeField] Animator _animator;
    public async void OnchapterChange()
    {

        exit.SetActive(!exit.activeSelf);

        PlayAndExecute("SelectChapter").Forget();
    }

    public async UniTaskVoid PlayAndExecute(string animName)
    {

        _animator.Play(animName);

        await UniTask.Yield(PlayerLoopTiming.Update);

        var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        await UniTask.Delay(System.TimeSpan.FromSeconds(stateInfo.length));


        ExecuteNextLogic();
    }
    public void ExecuteNextLogic()
    {
        title.SetActive(!title.activeSelf);

        chaptercollect.SetActive(!chaptercollect.activeSelf);
    }
}
