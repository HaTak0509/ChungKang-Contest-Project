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
    private bool isKnockback = false;
    private bool isInvincible = false;

    public bool IsKnockback => isKnockback;
    public bool IsInvincible => isInvincible;
    public bool IsStunnedOrKnockback => isKnockback || isInvincible;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Log("E키를 누르면 넉백(일단 오른쪽으로만 밀림)");
    }

    private void Start()
    {
        if (startInvincible) SetInvincible(999f);
    }

    public void TakePush(Vector2 pushPosition, bool applyKnockback = true)
    {
        if (applyKnockback)
            ApplyKnockback(pushPosition);
    }

    private void ApplyKnockback(Vector2 dirOrPos)
    {
        if (isKnockback) return;

        Vector2 dir = dirOrPos;
        if (dirOrPos == (Vector2)transform.position)
            dir = ((Vector2)transform.position - dirOrPos).normalized;

        if (Mathf.Abs(dir.y) < 0.3f) dir.y = 0.4f;

        rb.velocity = Vector2.zero;
        rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);

        StopAllCoroutines();
        StartCoroutine(KnockbackRoutine());
    }

    private IEnumerator KnockbackRoutine()
    {
        isKnockback = true;
        isInvincible = true;
        yield return new WaitForSeconds(knockbackDuration);
        isKnockback = false;
        isInvincible = false;
        onInvincibleEnd?.Invoke();
    }

    public void SetInvincible(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(InvincibleRoutine(duration));
    }

    private IEnumerator InvincibleRoutine(float duration)
    {
        isInvincible = true;
        yield return new WaitForSeconds(duration);
        isInvincible = false;
        onInvincibleEnd?.Invoke();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            TakePush(transform.position + Vector3.left);
    }
}