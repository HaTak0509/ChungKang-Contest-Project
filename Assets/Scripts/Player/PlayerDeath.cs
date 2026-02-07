using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private Animator _animator;
    private bool _Death = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnDeath()
    {
        if(_Death)return;

        _Death = true;
        _animator.SetTrigger(AnimationStrings.IsDeath);

        GetComponent<PlayerController>().allLimit = true;
        LevelManager.Instance.OnReset();
    }
}
