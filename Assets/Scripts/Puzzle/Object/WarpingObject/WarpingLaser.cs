using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WarpingLaser : MonoBehaviour, WarpingInterface
{
    [SerializeField] private List<GameObject> _hideObjects = new();
    [SerializeField] private GameObject laser;
    [SerializeField] private GameObject tileOb;

    private List<SpriteRenderer> _puzzleObjectSp = new();
    private List<GameObject> _puzzleObjects = new();

    private TransparencyUI _transparencyUI;
    private PlayerColor _playerColor;

    private Tilemap _tileMap;
    private TilemapCollider2D _tileCol;

    private bool _objectActive;
    private bool _playerActive;

    private void Start()
    {
        _tileMap = tileOb.GetComponent<Tilemap>();
        _tileCol = tileOb.GetComponent<TilemapCollider2D>();
    }

    private void OnEnable()
    {
        _objectActive = true;
    }

    private void Update()
    {
        if (!_objectActive) return;

        _objectActive = false;
        _playerActive = true;

        LowerAllTransparencyAndDisableColliders().Forget();

        foreach (var hideOb in _hideObjects)
            hideOb.SetActive(true);
    }

    private void OnDisable()
    {
        RestoreImmediately();

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
        if (collision.CompareTag("Player") && _playerActive)
        {
            if (_playerColor != null) return;

            _transparencyUI = collision.GetComponentInChildren<TransparencyUI>();
            _playerColor = collision.GetComponent<PlayerColor>();

            _playerColor?.LowerTransparency().Forget();
            if (_transparencyUI != null)
                _transparencyUI.active = true;
        }
        else if (collision.CompareTag("PuzzleObject"))
        {
            var sr = collision.GetComponent<SpriteRenderer>();
            if (sr != null && !_puzzleObjectSp.Contains(sr))
            {
                _puzzleObjectSp.Add(sr);
                _puzzleObjects.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || _playerColor == null) return;

        _playerColor.UpperTransparency().Forget();
        _playerColor = null;

        if (_transparencyUI != null)
        {
            _transparencyUI.active = false;
            _transparencyUI = null;
        }
    }

    public void Warping()
    {
        RestoreImmediately();
        laser?.SetActive(true);
        gameObject.SetActive(false);
    }

    private void RestoreImmediately()
    {
        // Tilemap 상태
        _tileCol.isTrigger = false;
        tileOb.layer = LayerMask.NameToLayer("Ground");
        tileOb.tag = "PuzzleObject";
        _tileMap.color = Color.white;

        // 퍼즐 오브젝트
        foreach (var ob in _puzzleObjects)
        {
            if (ob == null) continue;
            foreach (var col in ob.GetComponents<Collider2D>())
                col.enabled = true;
        }
    }

    private async UniTaskVoid LowerAllTransparencyAndDisableColliders()
    {
        var tasks = new List<UniTask>();

        foreach (var sr in _puzzleObjectSp)
            if (sr != null)
                tasks.Add(FadeSprite(sr, 0.3f));

        if (_tileMap != null)
            tasks.Add(FadeTilemap(0.3f));

        await UniTask.WhenAll(tasks);

        foreach (var ob in _puzzleObjects)
        {
            if (ob == null) continue;
            foreach (var col in ob.GetComponents<Collider2D>())
                col.enabled = false;
        }

        _tileCol.isTrigger = true;
        tileOb.layer = LayerMask.NameToLayer("Default");
        tileOb.tag = "Untagged";
    }

    private async UniTask FadeSprite(SpriteRenderer sr, float target)
    {
        Color c = sr.color;
        while (!Mathf.Approximately(c.a, target))
        {
            c.a = Mathf.MoveTowards(c.a, target, 0.15f);
            sr.color = c;
            await UniTask.Delay(30);
        }
    }

    private async UniTask FadeTilemap(float target)
    {
        Color c = _tileMap.color;
        while (!Mathf.Approximately(c.a, target))
        {
            c.a = Mathf.MoveTowards(c.a, target, 0.15f);
            _tileMap.color = c;
            await UniTask.Delay(30);
        }
    }
}
