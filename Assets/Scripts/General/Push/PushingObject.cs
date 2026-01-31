using UnityEngine;

public class PushingObject : MonoBehaviour, IInteractable
{
    public bool isActive;

    private Rigidbody2D rb;
    private Pushing pushing;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() { }

    public void Interact()
    {
        isActive = !isActive;
        pushing.PushingDirection();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            pushing = collision.GetComponent<Pushing>();
        }
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
