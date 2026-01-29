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
    public bool moveLimit;
    public Vector2 moveInput;

    private InteractionSign interactionSign;
    private Pushing pushing;
    private Animator animator;

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
        pushing = GetComponent<Pushing>();
        animator = GetComponent<Animator>();

        interaction = false;
    }

    private void Update()
    {
        HandleInteraction();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (moveLimit) return;

        moveInput = context.ReadValue<Vector2>();

        if (pushing.isPushing)
        {
            if (Mathf.Sign(moveInput.x) != Mathf.Sign(pushing.pushingDirection))
            {
                moveInput = Vector2.zero;
            }
        }

        animator.SetBool(AnimationStrings.IsMoving, moveInput != Vector2.zero);
        movement.SetInput(moveInput);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (moveLimit) return;
        if (context.started) jump.TryJump();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (moveLimit) return;
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

        interaction = false;

        if (interactionSign == null) return;

        Transform target = interactionSign.GetCurrentTopTarget();
        if (target == null) return;

        if (target.TryGetComponent<IInteractable>(out var interactable))
        {
            interactable.Interact();
        }
    }
}