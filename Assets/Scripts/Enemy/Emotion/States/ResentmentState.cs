using Unity.VisualScripting;
using UnityEngine;

public class ResentmentState : Monster
{
    //*************************************************************
    // [ 코드 설명 ] :
    // 감정에 따른 여러 행동방식을 유사한 구조로 작성해서, 유지보수와
    // 확장성을 챙기기 위해 인터페이스 형식을 사용함
    //*************************************************************


    private CircleCollider2D _circleCollider2D;
    private Flotation _flotation;

    public void OnEnter(Monster monster)
    {

        if (monster.GetComponent<CircleCollider2D>() == null)
        {
            _circleCollider2D = monster.AddComponent<CircleCollider2D>();
            _circleCollider2D.radius = monster.InteractRange;
            _circleCollider2D.isTrigger = true;
        }
        else
        {
            _circleCollider2D = monster.GetComponent<CircleCollider2D>();
            _circleCollider2D.enabled = true;
            _circleCollider2D.isTrigger = true;
        }

        if (monster.GetComponent<Flotation>() == null)
        {
            _flotation = monster.AddComponent<Flotation>();
        }
        else
        {
            _flotation = monster.GetComponent<Flotation>();
            _flotation.enabled = true;
        }




    }
    public void OnAction(Monster monster)
    {

    }
    public void UpdateState(Monster monster)
    {


    }

    public void OnExit(Monster monster)
    {

        _circleCollider2D.enabled = false;
        _flotation.enabled = false;


    }



}