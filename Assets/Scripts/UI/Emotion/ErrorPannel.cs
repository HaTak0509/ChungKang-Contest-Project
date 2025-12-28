using TMPro;
using UnityEngine;

public class ErrorPannel : MonoBehaviour
{
    //*************************************************************
    // [ 코드 설명 ] :
    // 에러 메시지 애니메이션 실행 및 텍스트 표시 
    //
    //*************************************************************
    
    private Animator _animator;
    public TMP_Text ErrorText;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.Play("OutError", 0, 1f);

        PlayerEmotionInventory.OnErrorPannel += OnPannel;
    }

    void OnDestroy()
    {
        //메모리 누수 방지를 위해 오브젝트 제거시 이벤트 구독 해제
        PlayerEmotionInventory.OnErrorPannel -= OnPannel;
    }


    private void OnPannel(string Error)
    {
        ErrorText.text = Error;
        _animator.SetTrigger("In");
    }







}
