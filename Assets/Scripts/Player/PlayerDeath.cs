using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnDeath()
    {
        _animator.SetBool(AnimationStrings.IsDeath, true);
        LevelReset.Instance.OnReset();
    }
}
