

public class JoyState : IEmotionState
{
    //*************************************************************
    // [ 코드 설명 ] :
    // 감정에 따른 여러 행동방식을 유사한 구조로 작성해서, 유지보수와
    // 확장성을 챙기기 위해 인터페이스 형식을 사용함
    //*************************************************************




    public EmotionType Type => EmotionType.Joy;

    public void OnEnter(Monster monster)
    {
        //monster.CanBePushed = true;
        //monster.EnableDash = true;
        //monster.SetMovementStyle(MovementStyle.FreeRoam);
    }

    public void UpdateState(Monster monster)
    {
        // 근처에 플레이어 있으면 활력 회복
        //monster.Energy += Time.deltaTime;
    }

    public void OnExit(Monster monster) { }
}