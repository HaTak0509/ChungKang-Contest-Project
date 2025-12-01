using UnityEngine;

public class HandUIController : MonoBehaviour
{

    //*************************************************************
    // [ 코드 설명 ] :
    // 감정 변경 패널의 등장, 퇴장 애니메이션 담당
    //*************************************************************

    public static HandUIController Instance;


    private Animator _animator;
    void Awake()
    {
        Instance = this;
        _animator = GetComponent<Animator>();
        _animator.Play("OutController",0,1f);
    }


    public void InUI()
    {
        _animator.SetTrigger("In");
    }

    public void OutUI()
    {
        _animator.SetTrigger("Out");
    }

    
}
