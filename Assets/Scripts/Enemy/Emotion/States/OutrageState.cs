using UnityEngine;
public class OutrageState : IEmotionState
{
    //*************************************************************
    // [ 코드 설명 ] :
    // 감정에 따른 여러 행동방식을 유사한 구조로 작성해서, 유지보수와
    // 확장성을 챙기기 위해 인터페이스 형식을 사용함
    //*************************************************************

    public EmotionType Type => EmotionType.Outrage;


    private MonsterMovement _movement;

    public void OnEnter(Monster monster)
    {
        if (_movement == null)
            _movement = monster.GetComponent<MonsterMovement>();

        HumidityManager.Instance.DownHumidity();

    }

    public void UpdateState(Monster monster)
    {
        _movement.Move();


    }
    public void OnAction(Monster monster)
    {
     
    }
    public void OnExit(Monster monster) 
    {
        HumidityManager.Instance.UpHumidity();

    }
}