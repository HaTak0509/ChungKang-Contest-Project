using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public class WarpingLaser : MonoBehaviour
{
    public bool active;

    private List<SpriteRenderer> puzzleObjects = new List<SpriteRenderer>();
    private PlayerColor player;
    private CancellationTokenSource cts;

    private void OnEnable()
    {
        cts = new CancellationTokenSource();
    }

    private void Update()
    {
        if (active)
        {
            ObjectLowerTransparency();
            active = false;
        }
    }

    private void OnDisable()
    {
        ObjectUpperTransparency();

        if (player != null)
        {
            player.UpperTransparency().Forget();
            player = null;
        }

        puzzleObjects.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.GetComponent<PlayerColor>();
            player.LowerTransparency().Forget();
        }
        else if (collision.gameObject.CompareTag("PuzzleObject"))
        {
            puzzleObjects.Add(collision.gameObject.GetComponent<SpriteRenderer>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.UpperTransparency().Forget();
            player = null;
        }
        else if (collision.gameObject.CompareTag("PuzzleObject"))
        {
            ObjectUpperTransparency();
        }
    }

    private void ObjectUpperTransparency()
    {
        foreach (var puzzleSp in puzzleObjects)
        {
            TransparencyUpper(puzzleSp).Forget();
        }

        puzzleObjects.Clear();
    }

    private void ObjectLowerTransparency()
    {
        foreach (var puzzleSp in puzzleObjects)
        {
            TransparencyLower(puzzleSp).Forget();
        }
    }

    private async UniTask TransparencyUpper(SpriteRenderer puzzleObjects)
    {
        Color color = puzzleObjects.color;

        while (puzzleObjects.color.a < 1f)
        {
            RestartToken();
            color.a = Mathf.Min(1f, color.a + 0.1f);
            puzzleObjects.color = color;

            await UniTask.Delay(TimeSpan.FromSeconds(0.03f), cancellationToken: cts.Token);
        }
    }

    private async UniTask TransparencyLower(SpriteRenderer puzzleObjects)
    {
        Color color = puzzleObjects.color;

        while (puzzleObjects.color.a > 0.3f)
        {
            RestartToken();
            color.a = Mathf.Max(0.3f, color.a - 0.15f);
            puzzleObjects.color = color;

            await UniTask.Delay(TimeSpan.FromSeconds(0.03f), cancellationToken: cts.Token);
        }
    }

    private void RestartToken()
    {
        cts.Cancel();
        cts.Dispose();
        cts = new CancellationTokenSource();
    }
}
