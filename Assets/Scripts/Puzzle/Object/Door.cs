using UnityEngine;

public class Door : MonoBehaviour
{
    public bool currentState;

    private Animator animator;
    private new Collider2D collider;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        collider.isTrigger = false;
    }

    public void OpenDoor()
    {
        currentState = true;
        collider.isTrigger = true;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public void CloseDoor()
    {
        currentState = false;
        collider.isTrigger = false;
        gameObject.layer = LayerMask.NameToLayer("Ground");
    }
}