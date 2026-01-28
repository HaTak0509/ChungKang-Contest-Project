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

    private InteractionSign interactionSign;
    private Vector2 moveInput;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (!movement) movement = GetComponent<PlayerMovement>();
        if (!jump) jump = GetComponent<PlayerJump>();
        if (!dash) dash = GetComponent<PlayerDash>();
        if (!damageable) damageable = GetComponent<Damageable>();

        interactionSign = GetComponent<InteractionSign>();

        interaction = false;
    }

    private void Update()
    {
        HandleInteraction();
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

    private void HandleInteraction()
    {
        if (!interaction) return;

        interaction = false; // 한 번만 처리

        if (interactionSign == null) return;

        Transform target = interactionSign.GetCurrentTopTarget();
        if (target == null) return;

        if (target.TryGetComponent<IInteractable>(out var interactable))
        {
            interactable.Interact();
        }
    }
}