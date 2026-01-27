using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSign : MonoBehaviour
{
    [SerializeField] private GameObject exclamationMark;
    [SerializeField] private GameObject questionMark;
    [SerializeField] private float setHeight = 1f;
    [SerializeField] private float maxHeight = 0.5f;
    [SerializeField] private float duration = 0.3f;

    private Dictionary<Transform, GameObject> quInsMark = new();

    private bool isQuitting = false;

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isQuitting) return;
        if (collision.CompareTag("Crack") || collision.CompareTag("PuzzleObject"))
        {
            CreateQuMark(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isQuitting) return;
        if (collision.CompareTag("Crack") || collision.CompareTag("PuzzleObject"))
        {
            RemoveQuMark(collision.transform);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isQuitting) return;
        if (collision.gameObject.CompareTag("PuzzleObject") && collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            CreateQuMark(collision.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isQuitting) return;
        if (collision.gameObject.CompareTag("PuzzleObject") && collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            RemoveQuMark(collision.transform);
        }
    }

    private void CreateQuMark(Transform target)
    {
        if (quInsMark.ContainsKey(target)) return;

        GameObject mark = Instantiate(questionMark, target.position + Vector3.up * setHeight, Quaternion.identity);
        quInsMark.Add(target, mark);

        // 마크 나타나는 애니메이션 시작
        StartCoroutine(ShowQuMark(mark));
    }

    private void RemoveQuMark(Transform target)
    {
        if (!quInsMark.TryGetValue(target, out GameObject mark)) return;

        // 마크 제거 애니메이션 실행 후 제거
        StartCoroutine(HideQuMark(target, mark));
    }

    private IEnumerator ShowQuMark(GameObject mark)
    {
        float t = 0f;
        SpriteRenderer sr = mark.GetComponent<SpriteRenderer>();
        if (sr == null) yield break;

        Color c = sr.color;
        c.a = 0f;
        sr.color = c;

        Vector3 startPos = mark.transform.position;
        Vector3 endPos = startPos + Vector3.up * maxHeight;

        while (t < duration)
        {
            if (mark == null || isQuitting) yield break;

            t += Time.deltaTime;
            float n = t / duration;

            c.a = Mathf.Lerp(0f, 1f, n);
            sr.color = c;
            mark.transform.position = Vector3.Lerp(startPos, endPos, n);

            yield return null;
        }
    }

    private IEnumerator HideQuMark(Transform target, GameObject mark)
    {
        float t = 0f;
        SpriteRenderer sr = mark.GetComponent<SpriteRenderer>();
        if (sr == null) yield break;

        Color c = sr.color;

        Vector3 startPos = mark.transform.position;
        Vector3 endPos = startPos - Vector3.up * maxHeight;

        while (t < duration)
        {
            if (mark == null || isQuitting) yield break;

            t += Time.deltaTime;
            float n = t / duration;

            c.a = Mathf.Lerp(1f, 0f, n);
            sr.color = c;
            mark.transform.position = Vector3.Lerp(startPos, endPos, n);

            yield return null;
        }

        // 안전하게 제거
        Destroy(mark);
        quInsMark.Remove(target);
    }

    private void OnDisable()
    {
        // 모든 마크 즉시 제거 (안전)
        foreach (var mark in quInsMark.Values)
        {
            if (mark != null)
                Destroy(mark);
        }
        quInsMark.Clear();
    }
}
