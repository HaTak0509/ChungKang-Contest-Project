using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalLift : MonoBehaviour
{
    [SerializeField] private float speed = 3f;  // 이동 속도
    [SerializeField] private float horizontalX = 5f; // Inspector에서 땅에 닿을 Y 위치 설정 (예: -5)
    [SerializeField] private float decelerationPosition = 3f;
    [SerializeField] private float decelerationSpeed = 0.2f;

    public bool _calling;
    public bool _isMoving = false;

    private Vector2 _originalPos;  // 원래 위 위치
    private bool _atGoal = false;  // 토착 했는가
    private bool _playerOnLift = false;  // Player가 타고 있는지
    private bool _isStart = false;
    private Rigidbody2D _rb2D;
    private Rigidbody2D _playerRb2D;

    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _originalPos = transform.position; // 시작 위치를 원래 위치로 저장
    }

    void Update()
    {
        // F키 누르고, Player가 타고 있고, 이동 중이 아닐 때만 동작
        if ((Input.GetKeyDown(KeyCode.F) && _playerOnLift && !_isMoving) || (_calling && !_isMoving))
        {
            Vector2 target;
            if (_atGoal)
            {
                // 원래 자리로 돌아옴
                target = _originalPos;
                _atGoal = false;
            }
            else
            {
                // 옆으로 이동함
                target = new Vector2(horizontalX, _originalPos.y);
                _atGoal = true;
            }
            StartCoroutine(MoveTo(target));
        }
    }

    IEnumerator MoveTo(Vector2 target)
    {
        _isMoving = true;
        _calling = false;

        while (Vector2.Distance(_rb2D.position, target) > 0.01f)
        {
            Vector2 newPos = Vector2.MoveTowards(
                _rb2D.position,
                target,
                speed * Time.fixedDeltaTime
            );

            Vector2 delta = newPos - _rb2D.position;

            _rb2D.MovePosition(newPos);

            if (_playerOnLift && _playerRb2D != null)
            {
                _playerRb2D.position += delta;
            }

            yield return new WaitForFixedUpdate();
        }

        _rb2D.MovePosition(target);
        _isMoving = false;
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        _playerOnLift = true;
        _playerRb2D = collision.rigidbody;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        _playerOnLift = false;
        _playerRb2D = null;
    }
}
