using System.Collections;
using UnityEngine;

public class VerticalLift : MonoBehaviour, IInteractable
{
    [SerializeField] private float speed = 3f; 
    [SerializeField] private float verticalY = -5f; 
    [SerializeField] private float decelerationPosition = 3f;
    [SerializeField] private float decelerationSpeed = 0.2f;

    private bool _atBottom = false;
    private bool _playerOnLift = false;
    private bool _isStart = false;

    private Vector2 _originalPos;
    private Rigidbody2D _rb2D;
    private Rigidbody2D _playerRb2D;

    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _originalPos = transform.position;
    }

    public void Interact()
    {
        if (!_playerOnLift) return;

        StartLift();
    }

    public void CallFromRemote()
    {
        StartLift();
    }

    private void StartLift()
    {
        gameObject.tag = "Wall";
        _isStart = false;

        Vector2 target;

        if (_atBottom)
        {
            target = _originalPos;
            _atBottom = false;
        }
        else
        {
            target = new Vector2(_originalPos.x, verticalY);
            _atBottom = true;
        }

        StartCoroutine(MoveTo(target));
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

    IEnumerator MoveTo(Vector2 target)
    {
        while (Vector2.Distance(_rb2D.position, target) > decelerationPosition)
        {
            _rb2D.MovePosition(
                Vector2.MoveTowards(_rb2D.position, target, speed * Time.fixedDeltaTime)
            );

            HandlePlayerGravity();
            yield return new WaitForFixedUpdate();
        }

        while (Vector2.Distance(_rb2D.position, target) > 0.01f)
        {
            float remaining = Vector2.Distance(_rb2D.position, target);
            float slowFactor = Mathf.SmoothStep(decelerationSpeed, 1f, remaining);
            float step = speed * slowFactor * Time.fixedDeltaTime;

            _rb2D.MovePosition(
                Vector2.MoveTowards(_rb2D.position, target, step)
            );

            yield return new WaitForFixedUpdate();
        }

        _rb2D.MovePosition(target);
        gameObject.tag = "PuzzleObject";
    }

    private void HandlePlayerGravity()
    {
        if (_playerRb2D == null) return;

        if (!_isStart)
        {
            _playerRb2D.gravityScale = 100;
            _isStart = true;
        }
        else
        {
            _playerRb2D.gravityScale = 1;
        }
    }
}