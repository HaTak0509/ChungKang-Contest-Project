using UnityEngine;

public class PushingObject : MonoBehaviour
{
    public bool isPushing;
    public float pushForce = 5f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Push(Vector2 direction)
    {
        rb.velocity = new Vector2(direction.x * pushForce, rb.velocity.y);
    }

    public void Stop()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }
}
