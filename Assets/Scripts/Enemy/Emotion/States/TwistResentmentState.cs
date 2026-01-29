using System.Collections.Generic;
using UnityEngine;

public class TwistResentmentState : IEmotionState //뒤틀린 원망
{
    //*************************************************************
    // [ 코드 설명 ] :
    // 감정에 따른 여러 행동방식을 유사한 구조로 작성해서, 유지보수와
    // 확장성을 챙기기 위해 인터페이스 형식을 사용함
    //*************************************************************

    public EmotionType Type => EmotionType.TwistResentment;


    private MonsterMovement _movement;
    private DrawSensingRange _lineRenderer;
    private LayerMask _playerLayer = LayerMask.GetMask("Player");

    private HashSet<string> _isplayer = new HashSet<string>();
    PlayerScale _playerScale;


    public void OnEnter(Monster monster)
    {
        if (_movement == null)
            _movement = monster.GetComponent<MonsterMovement>();

        if (_lineRenderer == null) //라인렌더러 찾아서, 켜고 색깔 지정하기
        {
            _lineRenderer = monster.GetComponent<DrawSensingRange>();
            _lineRenderer.OnLine();

            if (ColorUtility.TryParseHtmlString(Emotion.Get(monster._CurrentEmotion).hexColor, out Color newColor))
            {
                Debug.Log(newColor.ToString());
                _lineRenderer.Draw(monster.InteractRange, newColor);
            }
        }
    }
    public void OnAction(Monster monster)
    {

    }
    public void UpdateState(Monster monster)
    {
        _movement.Move();

        Collider2D hit = Physics2D.OverlapCircle(monster.transform.position, monster.InteractRange, _playerLayer);

        if (hit != null)
        {
            _playerScale = hit.GetComponent<PlayerScale>();
            _playerScale._isDown = true;
            _isplayer.Add("Player");
        }
        else
        {
            if (_isplayer.Contains("Player"))
            {
                _playerScale._isDown = false;
                _isplayer.Remove("Player");
            }
        }
    }

    public void OnExit(Monster monster)
    {
        _lineRenderer.OffLine();
    }


}