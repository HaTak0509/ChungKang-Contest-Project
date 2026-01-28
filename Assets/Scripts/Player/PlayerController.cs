using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set;}

    [SerializeField] private GameObject explorationRange;   

    public PlayerMovement movement;
    public PlayerJump jump;
    public PlayerDash dash;
    public Damageable damageable;
    public bool interaction;

    private Vector2 moveInput;

    private void Awake()
    {
        if (!movement) movement = GetComponent<PlayerMovement>();
        if (!jump) jump = GetComponent<PlayerJump>();
        if (!dash) dash = GetComponent<PlayerDash>();
        if (!damageable) damageable = GetComponent<Damageable>();

        interaction = false;
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

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started) dash.TryDash();
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (context.started) interaction = true;
    }

    public void OnExploration(InputAction.CallbackContext context)
    {
        if (context.started) explorationRange.SetActive(!explorationRange.activeSelf);
    }
}