using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class LiftController : MonoBehaviour
{
    [SerializeField] private float speed = 3f;  // 이동 속도
    [SerializeField] private float downY = -5f; // Inspector에서 땅에 닿을 Y 위치 설정 (예: -5)

    private Vector2 _originalPos;  // 원래 위 위치
    private bool _atBottom = false;  // 아래에 있는지 여부
    private bool _playerOnLift = false;  // Player가 타고 있는지
    private bool _isMoving = false;  // 이동 중인지
    private bool _isStart = false;
    private Rigidbody2D _rb2D;
    private Rigidbody2D _playerRb2D;

    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _originalPos = transform.position;  // 시작 위치를 원래 위치로 저장
    }

    void Update()
    {
        // F키 누르고, Player가 타고 있고, 이동 중이 아닐 때만 동작
        if (Input.GetKeyDown(KeyCode.F) && _playerOnLift && !_isMoving)
        {
            Vector2 target;
            if (_atBottom)
            {
                // 아래에서 위로 올라감
                target = _originalPos;
                _atBottom = false;
            }
            else
            {
                // 위에서 아래로 내려감
                target = new Vector2(_originalPos.x, downY);
                _atBottom = true;
            }
            StartCoroutine(MoveTo(target));
        }
    }

    IEnumerator MoveTo(Vector2 target)
    {
        _isMoving = true;

        while (Vector2.Distance(_rb2D.position, target) > 2f)
        {
            _rb2D.MovePosition(Vector2.MoveTowards(_rb2D.position, target, speed * Time.fixedDeltaTime));

            if (!_isStart && _playerRb2D != null)
            {
                _playerRb2D.gravityScale = 50;
                _isStart = true;
            }
            else if (_isStart && _playerRb2D != null)
            {
                _playerRb2D.gravityScale = 1;
            }

            yield return new WaitForFixedUpdate();
        }

        while (Vector2.Distance(_rb2D.position, target) > 0.01f)
        {
            float remaining = Vector2.Distance(_rb2D.position, target);

            // 0~1 사이 비율 (2f에서 시작해서 0에 가까워짐)
            float t = Mathf.Clamp01(remaining / 2f);

            // 감속 커브 (부드럽게)
            float slowFactor = Mathf.SmoothStep(0.2f, 1f, remaining);

            float step = speed * slowFactor * Time.fixedDeltaTime;

            _rb2D.MovePosition(
                Vector2.MoveTowards(_rb2D.position, target, step)
            );

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