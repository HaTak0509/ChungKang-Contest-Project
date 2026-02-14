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
            Debug.Log(_stuckObject);
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
        Collider2D[] results = new Collider2D[20];
        var filter = new ContactFilter2D().NoFilter();
        filter.useTriggers = true;
        Debug.Log(123);

        int count = Physics2D.OverlapCollider(
            GetComponent<Collider2D>(),
            filter,
            results
        );


        for (int i = 0; i < count; i++)
        {
            if (results[i] == null) continue;

            if (results[i].TryGetComponent<WarpingInterface>(out var warp))
            {
                warp.Warping();
                break;
            }
        }
    }

    private async UniTask Cooldown()
    {
        await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
        _warpActive = false;
    }
}
