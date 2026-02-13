using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private bool _Death = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (_Death)
        {
            _spriteRenderer.flipY = false;
        }
    }

    public void OnDeath()
    {
        if(_Death)return;

        SoundManager.Instance.PlaySFX("Death", SoundManager.SoundOutput.SFX, 0.7f);

        _Death = true;
        _animator.SetTrigger(AnimationStrings.IsDeath);
        _animator.SetBool(AnimationStrings.IsDeathBool,true);


        GetComponent<PlayerController>().allLimit = true;
        LevelManager.Instance.OnReset();
    }
}
