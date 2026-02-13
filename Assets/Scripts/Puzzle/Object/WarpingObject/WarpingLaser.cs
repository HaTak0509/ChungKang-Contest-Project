using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WarpingLaser : MonoBehaviour, WarpingInterface
{
    [SerializeField] private List<GameObject> _hideObjects = new();
    [SerializeField] private GameObject laser;
    [SerializeField] private GameObject hidingTile;

    private readonly List<SpriteRenderer> _puzzleObjectSp = new();
    private readonly List<GameObject> _puzzleObjects = new();

    private TransparencyUI _transparencyUI;
    private PlayerColor _playerColor;

    private Tilemap _tileMap;
    private TilemapCollider2D _tileCol;

    private bool _playerActive;
    private bool _fadeStarted;

    private CancellationTokenSource _cts;

    private void Start()
    {
        if (hidingTile == null) return;

        _tileMap = hidingTile.GetComponent<Tilemap>();
        _tileCol = hidingTile.GetComponent<TilemapCollider2D>();
    }

    private void OnEnable()
    {
        _cts = new CancellationTokenSource();
        _fadeStarted = false;
    }

    private void OnDisable()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;

        _playerColor = null;

        foreach (var hideOb in _hideObjects)
            hideOb.SetActive(false);

        if (_transparencyUI != null)
        {
            _transparencyUI.active = false;
            _transparencyUI = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_playerColor != null) return;

            _playerActive = true;

            _transparencyUI = collision.GetComponentInChildren<TransparencyUI>();
            _playerColor = collision.GetComponent<PlayerColor>();

            _playerColor?.LowerTransparency().Forget();

            if (_transparencyUI != null)
                _transparencyUI.active = true;
        }

        if (collision.CompareTag("PuzzleObject"))
        {
            var sr = collision.GetComponentInChildren<SpriteRenderer>();
            if (sr == null) return;
            if (sr.GetComponent<CrackActivator>()) return;

            if (!_puzzleObjectSp.Contains(sr))
            {
                _puzzleObjectSp.Add(sr);
                _puzzleObjects.Add(collision.gameObject);
                Debug.Log(sr);
            }

            if (!_fadeStarted)
            {
                _fadeStarted = true;
                StartFadeDown().Forget();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || _playerColor == null) return;

        _playerActive = false;

        _playerColor.UpperTransparency().Forget();
        _playerColor = null;

        if (_transparencyUI != null)
        {
            _transparencyUI.active = false;
            _transparencyUI = null;
        }
    }

    public async void Warping()
    {
        if (_cts == null) return;

        await FadeUpAll(_cts.Token);

        RestoreCollidersAndTile();

        if (laser != null)
        {
            laser?.SetActive(true);
        }

        gameObject.SetActive(false);
    }

    private async UniTaskVoid StartFadeDown()
    {
        if (_cts == null) return;
        await LowerAllTransparencyAndDisableColliders(_cts.Token);
    }

    private async UniTask LowerAllTransparencyAndDisableColliders(CancellationToken token)
    {
        var tasks = new List<UniTask>();

        foreach (var sr in _puzzleObjectSp)
            if (sr != null)
                tasks.Add(FadeSprite(sr, 0.3f, token));

        if (_tileMap != null)
            tasks.Add(FadeTilemap(0.3f, token));

        await UniTask.WhenAll(tasks);

        foreach (var ob in _puzzleObjects)
        {
            if (ob == null) continue;
            foreach (var col in ob.GetComponents<Collider2D>())
                col.enabled = false;
        }

        if (hidingTile != null)
        {
            _tileCol.isTrigger = true;
            hidingTile.layer = LayerMask.NameToLayer("Default");
            hidingTile.tag = "Untagged";
        }

        foreach (var hideOb in _hideObjects)
            hideOb.SetActive(true);
    }

    private async UniTask FadeUpAll(CancellationToken token)
    {
        var tasks = new List<UniTask>();

        foreach (var sr in _puzzleObjectSp)
            if (sr != null)
                tasks.Add(FadeSprite(sr, 1f, token));

        if (_tileMap != null)
            tasks.Add(FadeTilemap(1f, token));

        await UniTask.WhenAll(tasks);
    }

    private void RestoreCollidersAndTile()
    {
        foreach (var ob in _puzzleObjects)
        {
            if (ob == null) continue;
            foreach (var col in ob.GetComponents<Collider2D>())
                col.enabled = true;
        }

        if (hidingTile != null)
        {
            _tileCol.isTrigger = false;
            hidingTile.layer = LayerMask.NameToLayer("Ground");
            hidingTile.tag = "PuzzleObject";
        }
    }

    private async UniTask FadeSprite(SpriteRenderer sr, float target, CancellationToken token)
    {
        while (!Mathf.Approximately(sr.color.a, target))
        {
            token.ThrowIfCancellationRequested();

            var c = sr.color;
            c.a = Mathf.MoveTowards(c.a, target, 0.15f);
            sr.color = c;

            await UniTask.Delay(30, cancellationToken: token);
        }
    }

    private async UniTask FadeTilemap(float target, CancellationToken token)
    {
        while (!Mathf.Approximately(_tileMap.color.a, target))
        {
            token.ThrowIfCancellationRequested();

            var c = _tileMap.color;
            c.a = Mathf.MoveTowards(c.a, target, 0.15f);
            _tileMap.color = c;

            await UniTask.Delay(30, cancellationToken: token);
        }
    }
}
