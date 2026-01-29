using UnityEngine;

public class Pushing : MonoBehaviour
{
    public bool isPushing;
    public bool pushing;
    public int pushingDirection;

    private PushingObject pushingObj;
    private Rigidbody2D playerRb;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (pushingObj == null || !pushingObj.isActive)
        {
            isPushing = false;
            return;
        }

        isPushing = pushingObj.isActive;

        float playerVelX = playerRb.velocity.x;

        if (Mathf.Abs(playerVelX) < 0.01f)
        {
            pushing = false;
            pushingObj.Stop();
            return;
        }

        float toBox = pushingObj.transform.position.x - transform.position.x;

        if (toBox > 0)
        {
            pushingDirection = 1;
        }
        else if (toBox < 0)
        {
            pushingDirection = -1;
        }


        if (Mathf.Sign(playerVelX) == pushingDirection)
        {
            pushing = true;
            pushingObj.CopyVelocity(playerVelX);
        }
        else
        {
            pushing = false;
            pushingObj.Stop();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PushingObject obj))
        {
            pushingObj = obj;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PushingObject obj))
        {
            if (pushingObj == obj)
            {
                pushingObj.Stop();
                pushingObj = null;
            }
        }
    }
}
    
