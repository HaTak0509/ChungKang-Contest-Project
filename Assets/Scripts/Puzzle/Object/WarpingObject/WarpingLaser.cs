using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WarpingLaser : MonoBehaviour, WarpingInterface
{
    [SerializeField] private List<GameObject> _hideObjects = new List<GameObject>();
    [SerializeField] private GameObject laser;
    [SerializeField] private SpriteRenderer timeMap;

    private List<SpriteRenderer> _puzzleObjectSp = new List<SpriteRenderer>();
    private List<GameObject> _puzzleObjects = new List<GameObject>();
    private CancellationTokenSource _cts;
    private TransparencyUI _transparencyUI;
    private PlayerColor _playerColor;

    private bool _objectActive;
    private bool _playerActive;

    private void OnEnable()
    {
        _cts = new CancellationTokenSource();
        _objectActive = true;
    }

    private void OnDisable()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;

        RestoreAllTransparencyAndColliders();

        _playerColor = null;

        if (_hideObjects.Count > 0)
        {
            foreach (var hideOb in _hideObjects)
            {
                hideOb.gameObject.SetActive(false);
            }
        }

        if (_transparencyUI != null)
        {
            _transparencyUI.active = false;
            _transparencyUI = null;
        }
    }

    private void Update()
    {
        if (_objectActive)
        {
            LowerAllTransparencyAndDisableColliders();
            _objectActive = false;
            _playerActive = true;

            if (_hideObjects.Count > 0)
            {
                foreach (var hideOb in _hideObjects)
                {
                    hideOb.gameObject.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _playerActive)
        {
            if (_playerColor != null) return;

            _transparencyUI = collision.GetComponentInChildren<TransparencyUI>();
            _playerColor = collision.GetComponent<PlayerColor>();

            if (_playerColor != null)
                _playerColor.LowerTransparency().Forget();

            _transparencyUI.active = true;
        }
        else if (collision.CompareTag("PuzzleObject"))
        {
            var ob = collision.gameObject;
            var sp = collision.GetComponent<SpriteRenderer>();

            if (sp != null && ob != null && !_puzzleObjectSp.Contains(sp))
            {
                _puzzleObjectSp.Add(sp);
                _puzzleObjects.Add(ob);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_playerColor != null)
            {
                _playerColor.UpperTransparency().Forget();
                _playerColor = null;

                if (_transparencyUI != null)
                {
                    _transparencyUI.active = false;
                    _transparencyUI = null;
                }
            }
        }
    }

    public void Warping()
    {
        if (laser != null)
        {
            laser.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    private async void LowerAllTransparencyAndDisableColliders()
    {
        if (_puzzleObjectSp.Count == 0 && timeMap == null) return;

        var puzzleSp = _puzzleObjectSp.ToArray();
        var puzzleOb = _puzzleObjects.ToArray();

        var tasks = new List<UniTask>();

        foreach (var sr in puzzleSp)
        {
            if (sr != null)
                tasks.Add(TransparencyLowerInstant(sr, _cts.Token));
        }

        if (timeMap != null)
            tasks.Add(TimeMapLower(_cts.Token));

        await UniTask.WhenAll(tasks);

        foreach (var ob in puzzleOb)
        {
            var allColliders = ob.GetComponents<Collider2D>();
            foreach (var col in allColliders)
            {
                if (col != null) col.enabled = false;
            }
        }
    }

    private async void RestoreAllTransparencyAndColliders()
    {
        if (_puzzleObjectSp.Count == 0 && timeMap == null) return;

        var puzzleSP = _puzzleObjectSp.ToArray();
        var puzzleOb = _puzzleObjects.ToArray();

        var tasks = new List<UniTask>();

        foreach (var Ob in puzzleOb)
        {
            if (Ob != null)
            {
                var allColliders = Ob.GetComponents<Collider2D>();
                foreach (var col in allColliders)
                {
                    if (col != null) col.enabled = true;
                }
            }
        }

        foreach (var sr in puzzleSP)
        {
            if (sr != null)
                tasks.Add(TransparencyUpperInstant(sr, _cts.Token));
        }

        if (timeMap != null)
            tasks.Add(TimeMapUpper(_cts.Token));

        await UniTask.WhenAll(tasks);
    }

    private async UniTask TransparencyLowerInstant(SpriteRenderer sr, CancellationToken token = default)
    {
        if (sr == null) return;

        Color c = sr.color;
        while (c.a > 0.3f)
        {
            c.a = Mathf.Max(0.3f, c.a - 0.15f);
            sr.color = c;
            await UniTask.Delay(TimeSpan.FromSeconds(0.03f), cancellationToken: token);
        }
    }

    private async UniTask TransparencyUpperInstant(SpriteRenderer sr, CancellationToken token = default)
    {
        if (sr == null) return;

        Color c = sr.color;
        while (c.a < 1f)
        {
            c.a = Mathf.Min(1f, c.a + 0.1f);
            sr.color = c;
            await UniTask.Delay(TimeSpan.FromSeconds(0.03f), cancellationToken: token);
        }
    }

    private async UniTask TimeMapLower(CancellationToken token = default)
    {
        if (timeMap == null) return;

        Color c = timeMap.color;
        while (c.a > 0.3f)
        {
            c.a = Mathf.Max(0.3f, c.a - 0.15f);
            timeMap.color = c;
            await UniTask.Delay(TimeSpan.FromSeconds(0.03f), cancellationToken: token);
        }
    }

    private async UniTask TimeMapUpper(CancellationToken token = default)
    {
        if (timeMap == null) return;

        Color c = timeMap.color;
        while (c.a < 1f)
        {
            c.a = Mathf.Min(1f, c.a + 0.1f);
            timeMap.color = c;
            await UniTask.Delay(TimeSpan.FromSeconds(0.03f), cancellationToken: token);
        }
    }
}
