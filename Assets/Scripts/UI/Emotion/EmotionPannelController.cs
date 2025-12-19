using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EmotionPannelController : MonoBehaviour
{
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.Play("Out", 0, 1f);

        MonsterEmotionManager.OnPannel += OnPannel;
    }

    void OnDestroy()
    {
        //메모리 누수 방지를 위해 오브젝트 제거시 이벤트 구독 해제
        MonsterEmotionManager.OnPannel -= OnPannel;
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





}
