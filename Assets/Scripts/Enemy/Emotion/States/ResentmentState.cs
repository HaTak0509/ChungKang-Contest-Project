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

        _lineRenderer.OnLine();

        if (ColorUtility.TryParseHtmlString(hexColor, out Color newColor))
        {
            Debug.Log(newColor.ToString());
            _lineRenderer.Draw(InteractRange, newColor);
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
        _lineRenderer.OffLine();
    }



}