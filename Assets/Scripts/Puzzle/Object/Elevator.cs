using UnityEngine;

public class Elevator : MonoBehaviour, IInteractable
{
    [SerializeField] private int nextLevel;

    public bool clear;

    private bool _action;

    public void Interact()
    {
        if (!_action) return;

        LevelManager.Instance.LoadLevel(nextLevel);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && clear)
            _action = true;
    }

    private void OnTriggerExitD(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            _action = false;
    }
}
