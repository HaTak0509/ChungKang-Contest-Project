using System;
using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] float dashForce = 10f;
    [SerializeField] float upgradeDashForce = 12f;
    [SerializeField] float dashTime = 0.15f;
    [SerializeField] float dashCooldown = 0.5f;
    
    public bool upgradeDash = false;
    public bool dashVitality = true;
    public bool dashing;

    private Rigidbody2D _rb2D;
    private PlayerFacing facing;
    private Pushing _pushing;
    private Damageable _damageable;
    private Animator _animator;
    private float prevDashForce;
    private bool canDash = true;

    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        facing = GetComponent<PlayerFacing>();
        _pushing = GetComponent<Pushing>();
        _damageable = GetComponent<Damageable>();
        _animator = GetComponent<Animator>();
        prevDashForce = dashForce;
    }

    private void Update()
    {
        if (upgradeDash) dashForce = upgradeDashForce;
        else dashForce = prevDashForce;
    }

    public void TryDash()
    {
        if (!canDash || _damageable.IsKnockback || _pushing.isPushing) return;
        if (dashVitality) StartCoroutine(DashRoutine());
    }

    private IEnumerator DashRoutine()
    {
        dashing = true;
        canDash = false;
        _damageable.SetInvincible(dashTime);
        _animator.SetTrigger(AnimationStrings.IsDash);

        float dir = facing.IsFacingRight ? 1f : -1f;
        _rb2D.velocity = new Vector2(dashForce * dir, _rb2D.velocity.y); // y 유지

        yield return new WaitForSeconds(dashTime);
        
        dashing = false;

        // 쿨타임은 별도 코루틴으로
        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;

        if (upgradeDash) dashVitality = true;
        else dashVitality = false;
    }
}