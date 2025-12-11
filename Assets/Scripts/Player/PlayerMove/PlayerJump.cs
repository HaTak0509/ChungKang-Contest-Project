using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] float jumpForce = 14f;

    private Rigidbody2D rb;
    private TouchingDetection ground;
    private Damageable health;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ground = GetComponent<TouchingDetection>();
        health = GetComponent<Damageable>();
    }

    public void TryJump()
    {
        if (!ground.IsGround) return;
        if (health.IsStunnedOrKnockback) return;

        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
}