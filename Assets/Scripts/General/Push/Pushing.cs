using UnityEngine;

public class Pushing : MonoBehaviour
{
    public bool isPush;

    private PushingObject _pushingOb;
    private Rigidbody2D playerRb;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (isPush && _pushingOb != null)
        {
            Vector2 moveDir = playerRb.velocity.normalized;
            _pushingOb.Push(moveDir);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PushingObject pushingObject))
        {
            _pushingOb = pushingObject;
            _pushingOb.isPushing = true;
            isPush = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PushingObject pushingObject))
        {
            pushingObject.Stop();
            pushingObject.isPushing = false;

            isPush = false;
            _pushingOb = null;
        }
    }
}
