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
    public bool allLimit;
    public bool moveLimit;
    public Vector2 moveInput;

    private InteractionSign interactionSign;
    private Pushing pushing;
    private Animator animator;

    private void Awake()
    {
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
        if (moveLimit || allLimit)
        {
            moveInput = Vector2.zero;
            return;
        }

        moveInput = context.ReadValue<Vector2>();

        animator.SetBool(AnimationStrings.IsMoving, moveInput != Vector2.zero);
        movement.SetInput(moveInput);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (moveLimit || allLimit) return;
        if (context.started) jump.TryJump();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (moveLimit || allLimit) return;
        if (context.started) dash.TryDash();
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (allLimit) return;
        if (context.started) interaction = true;

        SoundManager.Instance.PlaySFX("player_detaction", SoundManager.SoundOutput.SFX, 1);
    }

    public void OnExploration(InputAction.CallbackContext context)
    {
        if (allLimit) return;
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

    public void StopAnim()//이거 계속 걷는 문제 고치려고 넣기
    {
        animator.SetBool(AnimationStrings.IsMoving, false);
    }
}