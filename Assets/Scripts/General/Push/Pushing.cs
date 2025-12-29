using UnityEngine;

public class Pushing : MonoBehaviour
{
    public bool leftPush;
    public bool rightPush;

    private PushingObject _pushingOb;
    private Rigidbody2D playerRb;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (leftPush && _pushingOb != null)
        {
            Vector2 moveDir = playerRb.velocity.normalized;
            _pushingOb.Push(moveDir);

            if (!leftPush)
            {
                _pushingOb.Stop();
                _pushingOb = null;
            }
        }

        if (rightPush && _pushingOb != null)
        {
            Vector2 moveDir = playerRb.velocity.normalized;
            _pushingOb.Push(moveDir);

            if (!rightPush)
            {
                _pushingOb.Stop();
                _pushingOb = null;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PushingObject pushingObject))
        {
            _pushingOb = pushingObject;

            if (transform.position.x > _pushingOb.gameObject.transform.position.x)
            {
                leftPush = true;
            }
            else if (transform.position.x < _pushingOb.gameObject.transform.position.x)
            {
                rightPush = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PushingObject pushingObject))
        {
            if (transform.position.x > _pushingOb.gameObject.transform.position.x)
            {
                leftPush = false;
            }
            else if (transform.position.x < _pushingOb.gameObject.transform.position.x)
            {
                rightPush = false;
            }
        }
    }
}
