using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class Button : MonoBehaviour, IInteractable
{
    [SerializeField] private Door targetDoor;

    private bool _active;
    private bool _colldown;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _colldown = true;
    }

    public void Interact()
    {
        if (_active && !_colldown) return;
        _colldown = false;

        _animator.SetTrigger(AnimationStrings.OnButton);
        ButtonCollDown().Forget();
        
        if (targetDoor.currentState)
        {
            targetDoor.CloseDoor();
        }
        else
        {
            targetDoor.OpenDoor();
        }
        
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
