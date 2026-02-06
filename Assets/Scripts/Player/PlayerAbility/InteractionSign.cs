using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionSign : MonoBehaviour
{
    [SerializeField] private GameObject exclamationMark;
    [SerializeField] private GameObject questionMark;
    [SerializeField] private float setHeight = 1f;
    [SerializeField] private float maxHeight = 0.5f;
    [SerializeField] private float duration = 0.3f;

    private Dictionary<Transform, GameObject> quInsMark = new();
    private InteractionSequence sequence;
    private bool isQuitting = false;
    private Transform _currentTop;

    private void Awake()
    {
        sequence = GetComponent<InteractionSequence>();
    }

    private void Update()
    {
        if (isQuitting) return;
        if (sequence == null || quInsMark.Count == 0) return;

        // null이거나 태그가 맞지 않는 대상 정리
        List<Transform> toRemove = new();
        foreach (var target in quInsMark.Keys)
        {
            if (target == null || !target.CompareTag("PuzzleObject"))
                toRemove.Add(target);
        }

        foreach (var t in toRemove)
            RemoveQuMark(t);

        // 최우선 대상 갱신
        Transform newTop = sequence.GetTopPriority(quInsMark.Keys);
        if (newTop != _currentTop)
        {
            _currentTop = newTop;
            RefreshMarks();
        }
    }

    private void OnDestroy()
    {
        ClearAllMarks();
    }

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    private void OnDisable()
    {
        ClearAllMarks();
    }

    private void ClearAllMarks()
    {
        foreach (var mark in quInsMark.Values)
        {
            if (mark != null)
                Destroy(mark);
        }
        quInsMark.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isQuitting) return;
        if (collision.CompareTag("PuzzleObject"))
        {
            CreateMark(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isQuitting) return;
        if (collision.CompareTag("PuzzleObject"))
        {
            RemoveQuMark(collision.transform);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isQuitting) return;
        if (collision.gameObject.CompareTag("PuzzleObject") && 
            collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            CreateMark(collision.transform);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isQuitting) return;
        if (collision.gameObject.CompareTag("PuzzleObject") && 
            collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // 이미 존재하면 CreateMark에서 return되므로 중복 생성 방지됨
            CreateMark(collision.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isQuitting) return;
        if (collision.gameObject.CompareTag("PuzzleObject") && 
            collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            RemoveQuMark(collision.transform);
        }
    }

    private void CreateMark(Transform target)
    {
        if (quInsMark.ContainsKey(target)) return;

        // 현재 + 새 대상 중에서 누가 top인지 미리 계산
        var allTargets = quInsMark.Keys.Append(target);
        Transform top = sequence.GetTopPriority(allTargets);

        GameObject prefab = (top == target) ? exclamationMark : questionMark;

        GameObject mark = Instantiate(prefab, target.position + Vector3.up * setHeight, Quaternion.identity);
        quInsMark.Add(target, mark);

        Sign sign = mark.GetComponent<Sign>();
        if (sign != null)
        {
            sign.parent = gameObject;
            sign.signType = (prefab == exclamationMark) ? Sign.Type.Exclamation : Sign.Type.Question;
        }

        RefreshMarks();
        StartCoroutine(ShowQuMark(mark));
    }

    private void RemoveQuMark(Transform target)
    {
        if (!quInsMark.TryGetValue(target, out GameObject mark)) return;

        quInsMark.Remove(target);
        StartCoroutine(HideQuMark(mark));
        RefreshMarks();
    }

    private void RefreshMarks()
    {
        if (sequence == null || quInsMark.Count == 0) return;

        Transform top = sequence.GetTopPriority(quInsMark.Keys);
        List<Transform> targets = new List<Transform>(quInsMark.Keys);

        foreach (Transform target in targets)
        {
            if (!quInsMark.TryGetValue(target, out GameObject mark)) continue;

            bool shouldBeExclamation = (target == top);
            Sign.Type desiredType = shouldBeExclamation ? Sign.Type.Exclamation : Sign.Type.Question;

            Sign currentSign = mark.GetComponent<Sign>();
            if (currentSign == null) continue;

            if (currentSign.signType == desiredType)
                continue;

            // 타입이 다르면 교체
            Destroy(mark);

            GameObject newPrefab = shouldBeExclamation ? exclamationMark : questionMark;
            GameObject newMark = Instantiate(
                newPrefab,
                target.position + Vector3.up * setHeight,
                Quaternion.identity
            );

            quInsMark[target] = newMark;

            Sign newSign = newMark.GetComponent<Sign>();
            if (newSign != null)
            {
                newSign.parent = gameObject;
                newSign.signType = desiredType;
            }

            StartCoroutine(ShowQuMark(newMark));
        }
    }

    public Transform GetCurrentTopTarget()
    {
        if (sequence == null || quInsMark.Count == 0)
            return null;

        return sequence.GetTopPriority(quInsMark.Keys);
    }

    private IEnumerator ShowQuMark(GameObject mark)
    {
        if (mark == null) yield break;

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

    private IEnumerator HideQuMark(GameObject mark)
    {
        if (mark == null) yield break;

        float t = 0f;
        SpriteRenderer sr = mark.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Destroy(mark);
            yield break;
        }

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

        if (mark != null)
            Destroy(mark);
    }
}