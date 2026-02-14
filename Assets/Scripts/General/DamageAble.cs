using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Damageable : MonoBehaviour
{
    [SerializeField] private float knockbackForce = 12f;
    [SerializeField] private float knockbackDuration = 0.25f;
    [SerializeField] private bool startInvincible = false;

    public UnityEvent onDeath;
    public UnityEvent onInvincibleEnd;

    private Rigidbody2D rb;
    private bool _isKnockback = false;
    private bool _isInvincible = false;
    private Coroutine _invincibleCoroutine;
    private Animator _animator;

    public bool IsKnockback => _isKnockback;
    public bool IsInvincible => _isInvincible;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (startInvincible) SetInvincible(999f);
    }

    public void GameOver()
    {
        onDeath.Invoke();
    }

    public void TakePushFromPosition(Vector2 attackerPos) //힘을 따로 수정하지 않을 때, 기존 수치 사용
    {
        TakePushFromPosition(attackerPos, knockbackForce);
    }

    public void TakePushFromPosition(Vector2 attackerPos, float AttckForce) //힘을 따로 수정할때 사용 됨
    {
        Vector2 dir = ((Vector2)transform.position - attackerPos).normalized;
        ApplyKnockback(dir, AttckForce);
    }

    private void ApplyKnockback(Vector2 dir, float AttckForce)
    {
        if (_isKnockback) return;

        if (Mathf.Abs(dir.y) < 0.3f)
            dir.y = 0.4f;

        rb.velocity = Vector2.zero;
        rb.AddForce(dir * AttckForce, ForceMode2D.Impulse);

        if (_animator != null)
        {
            _animator.SetTrigger(AnimationStrings.IsKnockback);
        }

        StopCoroutine(KnockbackRoutine());
        StartCoroutine(KnockbackRoutine());
    }

    private IEnumerator KnockbackRoutine()
    {
        _isKnockback = true;
        _isInvincible = true;
        yield return new WaitForSeconds(knockbackDuration);
        _isKnockback = false;
        _isInvincible = false;
        onInvincibleEnd?.Invoke();
    }

    public void SetInvincible(float duration)
    {
        if (_invincibleCoroutine != null)
            StopCoroutine(_invincibleCoroutine);

        _invincibleCoroutine = StartCoroutine(InvincibleRoutine(duration));
    }

    private IEnumerator InvincibleRoutine(float duration)
    {
        _isInvincible = true;
        yield return new WaitForSeconds(duration);
        _isInvincible = false;
        onInvincibleEnd?.Invoke();
    }
}