using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class ObjectConfinement : MonoBehaviour
{
    private GameObject _stuckObject;
    private bool _warpActive;

    private void OnEnable()
    {
        _warpActive = true;
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
            !collision.CompareTag("HidingWall"))
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

    private async UniTask Cooldown()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        _warpActive = false;
    }
}
