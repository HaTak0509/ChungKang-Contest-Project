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
        Debug.Log("asdasd");
        _Player.Play("idleUI");
    }
}
