using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageAble : MonoBehaviour
{
    [SerializeField] private float knockBackDistance;

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
        Debug.Log("GetHit Button = E");
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            GetHit();
        }
    }

    public void GetHit()
    {
        if (_hit) return;
        StartCoroutine(KnockBackCoroutine(0.3f));
    }

    private IEnumerator KnockBackCoroutine(float duration)
    {
        Vector2 start = transform.position;
        Vector2 dir = transform.localScale.x > 0 ? Vector2.left : Vector2.right;
        Vector2 end = start + (dir * knockBackDistance);

        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime / duration;

            transform.position = Vector2.Lerp(start, end, t);

            yield return null;
        }
    }
}
