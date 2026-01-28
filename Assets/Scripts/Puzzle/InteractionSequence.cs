using System.Collections.Generic;
using UnityEngine;

public class InteractionSequence : MonoBehaviour
{
    public List<GameObject> sequence = new List<GameObject>();

    // 두 대상 중 우선순위가 더 높은 쪽 반환
    public Transform GetHigherPriority(Transform a, Transform b)
    {
        int indexA = sequence.IndexOf(a.gameObject);
        int indexB = sequence.IndexOf(b.gameObject);

        if (indexA == -1) return b;
        if (indexB == -1) return a;

        return indexA < indexB ? a : b;
    }

    // 여러 개 중에서 최우선 대상 하나 찾기 
    public Transform GetTopPriority(IEnumerable<Transform> targets)
    {
        Transform best = null;

        foreach (var t in targets)
        {
            if (best == null)
            {
                best = t;
                continue;
            }

            best = GetHigherPriority(best, t);
        }
        return best;
    }
}
