using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    [SerializeField] private GameObject _Chat;
    [SerializeField] private TMP_Text _Text;

    private KeyCode? _keyCode = null;
    private Animator _animator;

    public static DialogueManager Instance;
    
    private void Awake()
    {
        Instance = this;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(_keyCode != null)
        {
            if (Input.GetKeyUp((KeyCode)_keyCode))
            {
                _animator.Play("Off");
                PlayerController.Instance.allLimit = false;
            }
        }
    }

    public void UpdateFormattedText(string templateText, KeyCode templateCode)
    {
        if (_Text == null) return;

        // {key} 부분을 keyName 변수값으로 교체
        string result = templateText.Replace("{key}", templateCode.ToString());

        PlayerController.Instance.allLimit = true;
        PlayerController.Instance.StopAnim();
        _animator.Play("On");
        _keyCode = templateCode;
        PlayTyping(result).Forget();
    }

    [SerializeField] private float delayPerChar = 0.05f; // 글자당 대기 시간 (초)

    private CancellationTokenSource cts;

    // 호출 예시: PlayTyping("안녕하세요! 반전 필터 시스템입니다.").Forget();
    private async UniTaskVoid PlayTyping(string fullText)
    {
        // 이전 실행 중인 타이핑이 있다면 취소
        cts?.Cancel();
        cts = new CancellationTokenSource();

        _Text.text = ""; // 텍스트 초기화

        try
        {
            for (int i = 0; i < fullText.Length; i++)
            {
                // 한 글자씩 추가
                _Text.text += fullText[i];

                // 설정한 시간만큼 대기
                await UniTask.Delay(TimeSpan.FromSeconds(delayPerChar), cancellationToken: cts.Token);
            }
        }
        catch (OperationCanceledException)
        {
            // 작업 취소 시 처리 (필요한 경우)
            Debug.Log("타이핑 효과가 취소되었습니다.");
        }
    }

    private void OnDisable()
    {
        cts?.Cancel();
        cts?.Dispose();
    }


}