using UnityEngine;

public class OutrageState : Monster
{
    //*************************************************************
    // [ 코드 설명 ] :
    // 감정에 따른 여러 행동방식을 유사한 구조로 작성해서, 유지보수와
    // 확장성을 챙기기 위해 인터페이스 형식을 사용함
    //*************************************************************

    [Header("세부 속성")]
    public GameObject Fire;

    public override void OnEnter()
    {
        HumidityManager.Instance.DownHumidity();
    }


    public override void UpdateState()
    {
        _movement.Move();

        if(HumidityManager.Instance.Humidity < 0 && WaterRiseController.Instance.CurrentStep <= 0)
        {
            Fire.SetActive(true);
        }
        else
        {
            Fire.SetActive(false);
        }

    }


    public override void OnExit() 
    {
        HumidityManager.Instance.UpHumidity();
    }


    

}