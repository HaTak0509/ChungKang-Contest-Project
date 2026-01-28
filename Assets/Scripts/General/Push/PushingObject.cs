using UnityEngine;

public class PushingObject : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Player의 x velocity를 그대로 복사
    public void CopyVelocity(float playerVelX)
    {
        rb.velocity = new Vector2(playerVelX, rb.velocity.y);
    }

    public void Stop()
    {
        rb.velocity = new Vector2(0f, rb.velocity.y);
    }
}
