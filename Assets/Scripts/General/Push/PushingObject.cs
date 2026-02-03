using UnityEngine;

public class PushingObject : MonoBehaviour, IInteractable
{
    [SerializeField] private Rigidbody2D rb;

    public bool isActive;

    private Pushing _pushing;

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _pushing = collision.GetComponent<Pushing>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _pushing = null;
        }
    }
    public void Interact()
    {
        isActive = !isActive;

        if (isActive)
        {
            _pushing.PushingDirection();
        }
    }

    public void CopyVelocity(float velocity)
    {
        rb.velocity = new Vector2(velocity, rb.velocity.y);
    }

    public void Stop()
    {
        rb.velocity = Vector2.zero;
    }
}
