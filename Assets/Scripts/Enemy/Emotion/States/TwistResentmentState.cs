using UnityEngine;

public class TwistResentmentState : Monster, IInteractable
{
    //*************************************************************
    // [ 코드 설명 ] :
    // 감정에 따른 여러 행동방식을 유사한 구조로 작성해서, 유지보수와
    // 확장성을 챙기기 위해 인터페이스 형식을 사용함
    //*************************************************************



    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void UpdateState()
    {

    }

    public void Interact()
    {
        PlayerScale.Instance.TriggerScaleChange();
    }

    public override void OnExit()
    {

    }


}