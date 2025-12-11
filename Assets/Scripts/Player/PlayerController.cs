using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerMovement movement;
    public PlayerJump jump;
    public PlayerDash dash;
    public Damageable damageable;

    private Vector2 moveInput;

    private void Awake()
    {
        if (!movement) movement = GetComponent<PlayerMovement>();
        if (!jump) jump = GetComponent<PlayerJump>();
        if (!dash) dash = GetComponent<PlayerDash>();
        if (!damageable) damageable = GetComponent<Damageable>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        movement.SetInput(moveInput);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started) jump.TryJump();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started)
            movement.SetSprinting(true);
        else if (context.canceled)
            movement.SetSprinting(false);
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started) dash.TryDash();
    }
}