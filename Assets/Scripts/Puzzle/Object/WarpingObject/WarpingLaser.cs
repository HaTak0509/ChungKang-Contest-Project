using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WarpingLaser : MonoBehaviour
{
    public bool active;
    
    private List<SpriteRenderer> puzzleObjectSp = new List<SpriteRenderer>();
    private List<GameObject> puzzleObjects = new List<GameObject>();
    private CancellationTokenSource cts;
    private PlayerColor currentPlayer;

    private void OnEnable()
    {
        cts = new CancellationTokenSource();
    }

    private void OnDisable()
    {
        cts?.Cancel();
        cts?.Dispose();
        cts = null; 
        RestoreAllTransparencyAndColliders();
        currentPlayer = null;
    }

    private void Update()
    {
        if (active)
        {
            LowerAllTransparencyAndDisableColliders();
            active = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && active)
        {
            if (currentPlayer != null) return;
            currentPlayer = other.GetComponent<PlayerColor>();
            if (currentPlayer != null)
                currentPlayer.LowerTransparency().Forget();
        }
        else if (other.CompareTag("PuzzleObject"))
        {
            var ob = other.gameObject;
            var sp = other.GetComponent<SpriteRenderer>();
            
            if (sp != null && ob != null && !puzzleObjectSp.Contains(sp))
            {
                puzzleObjectSp.Add(sp);
                puzzleObjects.Add(ob);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (currentPlayer != null)
            {
                currentPlayer.UpperTransparency().Forget();
                currentPlayer = null;
            }
        }
    }

    private async void LowerAllTransparencyAndDisableColliders()
    {
        if (puzzleObjectSp.Count == 0) return;

        var puzzleSp = puzzleObjectSp.ToArray();
        var puzzleOb = puzzleObjects.ToArray();

        var tasks = new List<UniTask>();
        foreach (var sr in puzzleSp)
        {
            if (sr != null) tasks.Add(TransparencyLowerInstant(sr));
        }

        await UniTask.WhenAll(tasks);

        foreach (var ob in puzzleOb)
        {
            var allColliders = ob.GetComponents<Collider2D>();           // ∫ª¿Œ

            foreach (var col in allColliders)
            {
                if (col != null) col.enabled = false;
            }
        }
    }

    private async void RestoreAllTransparencyAndColliders()
    {
        if (puzzleObjectSp.Count == 0) return;

        var puzzleSP = puzzleObjectSp.ToArray();
        var puzzleOb = puzzleObjects.ToArray();

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
                tasks.Add(TransparencyUpperInstant(sr));
        }

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
}