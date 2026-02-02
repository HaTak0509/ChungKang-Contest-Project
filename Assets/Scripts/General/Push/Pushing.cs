using UnityEngine;

public class Pushing : MonoBehaviour
{
    public bool isPushing;
    public bool pushing;
    public int pushingDirection;

    private PushingObject pushingObj;
    private Rigidbody2D playerRb;
    private BoxCollider2D boxCol;

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
            boxCol.enabled = true;
            pushingObj.Stop();
            return;
        }

        if (Mathf.Sign(playerVelX) == pushingDirection)
        {
            pushing = true;
            boxCol.enabled = false;
            pushingObj.CopyVelocity(playerVelX);
        }
        else
        {
            pushing = false;
            boxCol.enabled = true;
            pushingObj.Stop();
        }
    }

    public void PushingDirection()
    {
        pushingDirection = ((int)Mathf.Sign(pushingObj.transform.position.x - transform.position.x));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PushingObject obj))
        {
            pushingObj = obj;
            boxCol = collision.GetComponent<BoxCollider2D>();
        }
    }
}
    
