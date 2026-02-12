using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
public class ClearSceneManager : MonoBehaviour
{
    public Animator _Player;
    private Animator _SceneAnimator;
    private bool _IsPlaying = false;

    [TextArea(2, 5)]
    public string[] lines; // 해당 상태의 대사


    void Start()
    {

        _SceneAnimator = GetComponent<Animator>();

        if (!_IsPlaying)
        {
            _IsPlaying = true;
            _SceneAnimator.Play("StartCutScene");
        }


        DialogueManager.Instance.StartDialogue(lines, KeyCode.None);


    }



    public void EndWalk()
    {
        _Player.Play("PlayerIdleUI");
        WaitHacking().Forget();
    }
    public async UniTask WaitHacking()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1.5f));

        _Player.Play("PlayerHackingUI");


        await UniTask.Yield();

        await UniTask.WaitUntil(() => _Player.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

       
        _Player.Play("PlayerIdleUI");
       
        await UniTask.Delay(TimeSpan.FromSeconds(1f));

        _Player.Play("PlayerDieUI");

    }

}
