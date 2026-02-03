using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class ObjectConfinement : MonoBehaviour
{
    private GameObject _stuckObject;
    private bool _isActive;

    private void OnEnable()
    {
        _isActive = true;
        Cooldown().Forget();
    }

    private void OnDisable()
    {
        if (!_isActive) return;
        ReleaseObject();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isActive) return;

        if (!collision.CompareTag("PuzzleObject") &&
            !collision.CompareTag("Enemy"))
            return;

        if (_stuckObject != null && !_stuckObject.activeSelf)
        {
            _stuckObject = collision.gameObject;
            return;
        }

        if (_stuckObject != null) return;

        _stuckObject = collision.gameObject;

        if (_stuckObject.TryGetComponent<WarpingInterface>(out var warpInteract))
            warpInteract.Warping();
    }

    private void ReleaseObject()
    {
        if (_stuckObject != null &&
            _stuckObject.TryGetComponent<WarpingInterface>(out var warpInteract))
            warpInteract.Warping();

        _stuckObject = null;
    }

    private async UniTask Cooldown()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        _isActive = false;
    }
}
