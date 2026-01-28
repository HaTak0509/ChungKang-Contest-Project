using UnityEngine;

public class PushingObject : MonoBehaviour
{
    private Rigidbody2D boxRb;

    private void Awake()
    {
        boxRb = GetComponentInParent<Rigidbody2D>();
    }

    public void Push(Vector2 velocity)
    {
        boxRb.velocity = new Vector2(velocity.x, boxRb.velocity.y);
    }

    public void Stop()
    {
        boxRb.velocity = new Vector2(0f, boxRb.velocity.y);
    }
}
