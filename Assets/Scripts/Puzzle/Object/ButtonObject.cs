using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class ButtonObject : MonoBehaviour, IInteractable
{
    [SerializeField] private Door targetDoor;
    [SerializeField] private CrackController crackController;

    private bool _active;
    private bool _colldown;

    private void Awake()
    {
        _colldown = true;
    }

    public void Interact()
    {
        if (_active && !_colldown) return;
        _colldown = false;

        ButtonCollDown().Forget();
        
        if (crackController != null)
        {
            crackController.CrackActive();
        }

        if (targetDoor != null)
        {
            if (targetDoor.currentState)
            {
                targetDoor.CloseDoor();
            }
            else
            {
                targetDoor.OpenDoor();
            }
        }

        SoundManager.Instance.PlaySFX("button_click", SoundManager.SoundOutput.SFX);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _active = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _active = false;
        }
    }

    private async UniTask ButtonCollDown()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.4f));
        _colldown = true;
    }
}
