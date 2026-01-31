using System.Collections;
using UnityEngine;

public class HorizontalLift : MonoBehaviour, IInteractable
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float horizontalX = 5f; 
    [SerializeField] private float decelerationPosition = 3f;
    [SerializeField] private float decelerationSpeed = 0.2f;

    public bool _isMoving = false;

    private bool _atGoal = false;
    private bool _playerOnLift = false;

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
        if (_isMoving) return;
        if (!_playerOnLift) return;

        StartLift();
    }

    public void CallFromRemote()
    {
        if (_isMoving) return;

        StartLift();
    }

    private void StartLift()
    {
        gameObject.tag = "Wall";

        Vector2 target;

        if (_atGoal)
        {
            target = _originalPos;
            _atGoal = false;
        }
        else
        {
            target = new Vector2(horizontalX, _originalPos.y);
            _atGoal = true;
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
        _isMoving = true;

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
        gameObject.tag = "PuzzleObject";
        _isMoving = false;
    }
}
