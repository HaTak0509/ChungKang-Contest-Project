using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public class PlayerColor : MonoBehaviour
{
    public Color color;

    private SpriteRenderer sprite;
    private CancellationTokenSource cts;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        cts = new CancellationTokenSource();
    }

    private void OnDisable()
    {
        cts.Cancel();
        cts.Dispose();
    }

    public async UniTask LowerTransparency()
    {
        RestartToken();

        color = sprite.color;

        try
        {
            while (color.a > 0.1f)
            {
                color.a = Mathf.Max(0.1f, color.a - 0.05f);
                sprite.color = color;

                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken : cts.Token);
            }
        }
        catch (OperationCanceledException)
        {
            // 정상 취소 → 아무것도 안 해도 됨
        }
    }

    public async UniTask UpperTransparency()
    {
        RestartToken();

        color = sprite.color;

        try
        {
            while (color.a < 1f)
            {
                color.a = Mathf.Min(1f, color.a + 0.2f);
                sprite.color = color;

                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken : cts.Token);
            }
        }
        catch (OperationCanceledException)
        {
            // 정상 취소
        }
    }

    private void RestartToken()
    {
        cts.Cancel(); // 이전 실행 멈춤 (멈추기만 하고 정리는 안함)
        cts.Dispose(); // 모든 정보 초기화 (ex: 타이머, 작업중인 정보, 콜백 리스트 초기화
        cts = new CancellationTokenSource();
    }
}
