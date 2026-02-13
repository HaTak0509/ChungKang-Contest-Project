using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class ChapterCollectButton : MonoBehaviour
{
    [SerializeField] GameObject title;
    [SerializeField] GameObject chaptercollect;
    [SerializeField] GameObject exit;
    [SerializeField] GameObject Player;
    [SerializeField] Animator _animator;
    public async void OnchapterChange()
    {

        exit.SetActive(!exit.activeSelf);
        PlayAndExecute("SelectChapter").Forget();
    }

    public async UniTaskVoid PlayAndExecute(string animName)
    {

        Player.GetComponent<Animator>().Play("UIMovement");
        _animator.Play(animName);

        await UniTask.Yield(PlayerLoopTiming.Update);

        var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        await UniTask.Delay(System.TimeSpan.FromSeconds(stateInfo.length));

        await FadeInFadeOut.instance.FadeInImage();

        ExecuteNextLogic();

        await FadeInFadeOut.instance.FadeOutImage();

    }
    public void ExecuteNextLogic()
    {
        Player.GetComponent<RectTransform>().anchoredPosition = new Vector3(-82.222168f, -432.146545f, 0);
        Player.GetComponent<Animator>().Play("PlayerIdleUI");

        title.SetActive(!title.activeSelf);

        chaptercollect.SetActive(!chaptercollect.activeSelf);
    }
}
