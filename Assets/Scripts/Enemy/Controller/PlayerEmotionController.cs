using System;
using UnityEngine;

public class PlayerEmotionController : MonoBehaviour
{
    //*************************************************************
    // [ 코드 설명 ] :
    // 적을 클릭하면 감정 패널 호출, 이후 감정 변경 상태임을 Bool로 확인
    // 이후 1,2,3,4또는 버튼 클릭으로 감정 변경 요청
    //
    // [ 주의점 ] :
    // 해당 클랫는 감정 주입 이벤트를 '발행(Invoke)'하는 역할만 함
    // 실제 구독은 Monster.cs가 진행함. | 자세한 부분은 Monster에서
    // 이 클래스에서 이벤트 구독이나 구독해제를 하지 않도록 주의
    //*************************************************************

    public static PlayerEmotionController Instance;

    public bool _IsControll { get; private set; } = false;





    void Awake()
    {
        Instance = this;
       
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _IsControll = !_IsControll;

            MonsterEmotionManager.OnPannel.Invoke(_IsControll);

            //Time.timeScale = _IsControll ? 0 : 1;
            //시간 정지는 어떻게 구현할지 고려

         
        }


    }

}