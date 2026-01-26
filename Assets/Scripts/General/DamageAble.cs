using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Damageable : MonoBehaviour
{
    [SerializeField] private float knockbackForce = 12f;
    [SerializeField] private float knockbackDuration = 0.25f;
    [SerializeField] private bool startInvincible = false;

    public UnityEvent onInvincibleEnd;

    private Rigidbody2D rb;
    private bool _isKnockback = false;
    private bool _isInvincible = false;
    private Coroutine _invincibleCoroutine;


    public bool IsKnockback => _isKnockback;
    public bool IsInvincible => _isInvincible;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Log("q키를 누르면 넉백(일단 오른쪽으로만 밀림)");
    }

    private void Start()
    {
        if (startInvincible) SetInvincible(999f);
    }

    public void TakePushFromPosition(Vector2 attackerPos)
    {
        Vector2 dir = ((Vector2)transform.position - attackerPos).normalized;
        ApplyKnockback(dir);
    }

    private void ApplyKnockback(Vector2 dir)
    {
        if (_isKnockback) return;

        if (Mathf.Abs(dir.y) < 0.3f)
            dir.y = 0.4f;

        rb.velocity = Vector2.zero;
        rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);

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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            TakePushFromPosition(transform.position + Vector3.left);
    }
}