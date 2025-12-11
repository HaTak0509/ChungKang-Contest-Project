using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] float dashForce = 24f;
    [SerializeField] float dashTime = 0.15f;
    [SerializeField] float dashCooldown = 0.8f;

    private Rigidbody2D rb;
    private PlayerFacing facing;
    private Damageable health;
    private bool canDash = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        facing = GetComponent<PlayerFacing>();
        health = GetComponent<Damageable>();
    }

    public void TryDash()
    {
        if (!canDash || health.IsStunnedOrKnockback) return;

        StartCoroutine(DashRoutine());
    }

    private IEnumerator DashRoutine()
    {
        canDash = false;
        health.SetInvincible(dashTime); // 대시 중 무적 (선택)

        float dir = facing.IsFacingRight ? 1f : -1f;

        yield return new WaitForSeconds(dashTime);

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}