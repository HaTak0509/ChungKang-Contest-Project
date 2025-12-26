using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] float dashForce = 24f;
    [SerializeField] float dashTime = 0.15f;
    [SerializeField] float dashCooldown = 0.8f;
    
    public bool dashVitality = true;

    private Rigidbody2D _rb2D;
    private PlayerFacing facing;
    private Damageable _damageable;
    private bool canDash = true;

    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        facing = GetComponent<PlayerFacing>();
        _damageable = GetComponent<Damageable>();
    }

    public void TryDash()
    {
        if (!canDash || _damageable.IsKnockback) return;
        if (dashVitality) StartCoroutine(DashRoutine());
        dashVitality = false;
    }

    private IEnumerator DashRoutine()
    {
        canDash = false;
        _damageable.SetInvincible(dashTime);

        float dir = facing.IsFacingRight ? 1f : -1f;
        _rb2D.velocity = new Vector2(dashForce * dir, _rb2D.velocity.y); // y 유지

        yield return new WaitForSeconds(dashTime);

        // 쿨타임은 별도 코루틴으로
        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}