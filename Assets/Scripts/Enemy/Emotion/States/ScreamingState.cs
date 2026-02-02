
public class ScreamiingState : Monster
{
    //*************************************************************
    // [ 코드 설명 ] :
    // 감정에 따른 여러 행동방식을 유사한 구조로 작성해서, 유지보수와
    // 확장성을 챙기기 위해 인터페이스 형식을 사용함
    //*************************************************************


    public override void OnEnter()
    {
        base.OnEnter();


        HumidityManager.Instance.UpHumidity();
    }

    public override void UpdateState()
    {

    }
    public override void OnExit() 
    {

        HumidityManager.Instance.DownHumidity();
    }

}