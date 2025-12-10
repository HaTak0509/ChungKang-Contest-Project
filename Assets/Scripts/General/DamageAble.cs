using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageAble : MonoBehaviour
{
    [SerializeField] private float knockBackForce;
    [SerializeField] private float knockBackDuration;

    private PlayerInput playerInput;
    private Rigidbody2D rb2D;
    private bool _hit = false;

    public bool Hit
    {
        get { return _hit; }
        set 
        {
            _hit = value;
        }
    }

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            if (_hit) return;
            KnockBack(knockBackForce);
            Debug.Log("GetHit Button = E");
        }
    }

    public void KnockBack(float force)
    {
        Vector2 dir = transform.localScale.x > 0 ? Vector2.left : Vector2.right;
        rb2D.AddForce(dir * force, ForceMode2D.Impulse);

        SetMoveLimit(true);
        _hit = true;

        StartCoroutine(KnockbackEnd());
    }
    private void SetMoveLimit(bool value)
    {
        if (playerInput != null)
            playerInput.moveLimit = value;
    }
    private IEnumerator KnockbackEnd()
    {
        yield return new WaitForSeconds(knockBackDuration);

        SetMoveLimit(false);
        _hit = false;
    }
}
