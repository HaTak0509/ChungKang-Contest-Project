using UnityEngine;
public class TwistJoyState : Monster
{
    //*************************************************************
    // [ 코드 설명 ] :
    // 감정에 따른 여러 행동방식을 유사한 구조로 작성해서, 유지보수와
    // 확장성을 챙기기 위해 인터페이스 형식을 사용함
    //*************************************************************


    public override void OnEnter()
    {
    }

    public override void UpdateState()
    {
        _movement.Move();

        Collider2D hit = Physics2D.OverlapCircle(transform.position, InteractRange, LayerMask.GetMask("Player"));

        if (hit != null)
        {
            if(hit.GetComponent<PlayerDash>().upgradeDash == false)
                _animator.SetTrigger("isAction");
            
            hit.GetComponent<PlayerDash>().upgradeDash = true;
        }
    }


    public override void OnExit()
    {

    }

}