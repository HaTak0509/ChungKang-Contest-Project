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
            _Player.Play("UIMovement");
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

        SoundManager.Instance.PlaySFX("Hacking", SoundManager.SoundOutput.SFX, 0.9f);
        await UniTask.Yield();

        await UniTask.WaitUntil(() => _Player.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

       
        _Player.Play("PlayerHurtUI");
       
        await UniTask.Delay(TimeSpan.FromSeconds(0.2f));

        FadeInFadeOut.instance.FadeIn();

        _Player.Play("PlayerDieUI");
        SoundManager.Instance.PlaySFX("Death", SoundManager.SoundOutput.SFX, 1);
        await UniTask.Delay(TimeSpan.FromSeconds(3f));

        LevelManager.Instance.LoadLevel(0);
    }

}
