using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkSpeed = 6f;
    [SerializeField] float sprintSpeed = 10f;

    private Rigidbody2D rb;
    private Damageable health;
    private PlayerFacing facing;
    private Vector2 moveInput;
    private bool isSprinting;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Damageable>();
        facing = GetComponent<PlayerFacing>();
    }

    public void SetInput(Vector2 input)
    {
        moveInput = input;
        if (input.sqrMagnitude > 0.01f)
            facing.FaceDirection(input.x);
    }

    public void SetSprinting(bool value) => isSprinting = value;

    private void FixedUpdate()
    {
        if (health.IsStunnedOrKnockback) // ³Ë¹é Á¦¾î
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        float speed = isSprinting ? sprintSpeed : walkSpeed;

        rb.velocity = new Vector2(moveInput.x * speed, rb.velocity.y);
    }
}