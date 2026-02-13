using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class ObjectConfinement : MonoBehaviour
{
    private GameObject _stuckObject;
    private bool _warpActive;

    private void OnEnable()
    {
        DelayedStart().Forget();
    }

    private async UniTaskVoid DelayedStart()
    {
        await UniTask.Yield(); // 한 프레임 대기

        _warpActive = true;
        TryWarpOverlappingObjects();
        Cooldown().Forget();
    }

    private void OnDisable()
    {
        ReleaseObject();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("PuzzleObject") &&
            !collision.CompareTag("Enemy") &&
            !collision.CompareTag("HidingWall") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Crack") || 
            collision.gameObject.layer == LayerMask.NameToLayer("CrackActivator"))
            return;

        if (_stuckObject != null && !_stuckObject.activeSelf)
        {
            _stuckObject = collision.gameObject;
            return;
        }

        if (_stuckObject == null)
        {
            _stuckObject = collision.gameObject;
        }
        else
        {
            float currentDist =
                (transform.position - _stuckObject.transform.position).sqrMagnitude;

            float newDist =
                (transform.position - collision.transform.position).sqrMagnitude;

            if (newDist < currentDist)
            {
                _stuckObject = collision.gameObject;
            }
        }

        if (!_warpActive) return;

        if (_stuckObject.TryGetComponent<WarpingInterface>(out var warpInteract))
        {
            warpInteract.Warping();
        }
    }

    private void ReleaseObject()
    {
        if (_stuckObject != null &&
            _stuckObject.TryGetComponent<WarpingInterface>(out var warpInteract))
        {
            warpInteract.Warping();
        }

        _stuckObject = null;
    }

    private void TryWarpOverlappingObjects()
    {
        Collider2D[] results = new Collider2D[8];
        int count = Physics2D.OverlapCollider(
            GetComponent<Collider2D>(),
            new ContactFilter2D().NoFilter(),
            results
        );

        for (int i = 0; i < count; i++)
        {
            var col = results[i];

            if (!col.CompareTag("PuzzleObject") &&
                !col.CompareTag("Enemy") &&
                !col.CompareTag("HidingWall"))
                continue;

            if (col.TryGetComponent<WarpingInterface>(out var warp))
            {
                warp.Warping();
                break; // 1회만
            }
        }
    }

    private async UniTask Cooldown()
    {
        await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
        _warpActive = false;
    }
}
