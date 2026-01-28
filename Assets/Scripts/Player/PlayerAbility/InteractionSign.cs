using System;
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

    private void Awake()
    {
        sequence = GetComponent<InteractionSequence>();
    }

    private void Update()
    {
        if (isQuitting) return;

        List<Transform> toRemove = new();

        foreach (var target in quInsMark.Keys)
        {
            if (target == null || !target.CompareTag("PuzzleObject"))
            {
                toRemove.Add(target);
            }
        }

        foreach (var t in toRemove)
        {
            RemoveQuMark(t);
        }
    }

    private void OnApplicationQuit()
    {
        isQuitting = true;
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
        if (collision.gameObject.CompareTag("PuzzleObject") && collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            CreateMark(collision.transform);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isQuitting) return;
        if (collision.gameObject.CompareTag("PuzzleObject") && collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            CreateMark(collision.transform);
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

    private void CreateMark(Transform target)
    {
        if (quInsMark.ContainsKey(target)) return;

        GameObject prefab = questionMark;

        // 현재 대상들 중 최우선인지 확인
        Transform top = sequence.GetTopPriority(quInsMark.Keys.Append(target));
        if (top == target)
        {
            prefab = exclamationMark;
        }

        GameObject mark = Instantiate(prefab, target.position + Vector3.up * setHeight, Quaternion.identity);
        quInsMark.Add(target, mark);

        RefreshMarks(); //  중요
        StartCoroutine(ShowQuMark(mark));
    }

    private void RemoveQuMark(Transform target)
    {
        if (!quInsMark.TryGetValue(target, out GameObject mark)) return;

        // 먼저 Dictionary에서 제거
        quInsMark.Remove(target);

        // 애니메이션은 오브젝트만 관리
        StartCoroutine(HideQuMark(mark));

        RefreshMarks();
    }

    private void RefreshMarks()
    {
        if (sequence == null || quInsMark.Count == 0) return;

        Transform top = sequence.GetTopPriority(quInsMark.Keys);

        // 키 목록 복사
        List<Transform> targets = new List<Transform>(quInsMark.Keys);

        foreach (Transform target in targets)
        {
            if (!quInsMark.TryGetValue(target, out GameObject mark)) continue;

            bool isTop = target == top;
            GameObject correctPrefab = isTop ? exclamationMark : questionMark;

            // 이미 맞는 타입이면 패스
            if (mark != null && mark.name.Contains(correctPrefab.name))
                continue;

            Destroy(mark);

            GameObject newMark = Instantiate(
                correctPrefab,
                target.position + Vector3.up * setHeight,
                Quaternion.identity
            );

            quInsMark[target] = newMark;
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

        Destroy(mark);
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
