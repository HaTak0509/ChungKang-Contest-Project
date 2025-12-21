using TMPro;
using UnityEngine;

public class EmotionPannelController : MonoBehaviour
{
    private Animator _animator;
    public TMP_Text EmotionText;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.Play("Out", 0, 1f);

        PlayerEmotionInventory.OnPannel += OnPannel;
    }

    void OnDestroy()
    {
        //메모리 누수 방지를 위해 오브젝트 제거시 이벤트 구독 해제
        PlayerEmotionInventory.OnPannel -= OnPannel;
    }


    private void OnPannel(bool IsOpen)
    {

        if (IsOpen)
        {
            _animator.SetTrigger("In");
            
        }
        else
        {
            _animator.SetTrigger("Out");
        }
    }

    public void EmotionTextChange(string emotion)//만약 색깔까지 추가할꺼면 Emotion 통으로 가져오기
    {
        EmotionText.text = emotion;
    }





}
