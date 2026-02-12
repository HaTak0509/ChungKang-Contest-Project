using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject _Chat;
    [SerializeField] private TMP_Text _Text;
    [SerializeField] private float delayPerChar = 0.05f;
    [SerializeField] private float delayNextChar = 1f; // 자동 넘김 대기 시간

    private KeyCode? _keyCode = null;
    private Animator _animator;
    private CancellationTokenSource cts;
    private bool _isChat = false;

    private string[] _dialogueLines;
    private int _currentIndex = 0;
    private bool _isTyping = false;
    private string _currentFullText;

    public static DialogueManager Instance;

    private void Awake()
    {
        Instance = this;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // 채팅 중이고, 입력해야 할 키가 설정되어 있을 때만 Update에서 키 입력을 체크합니다.
        if (_isChat && _keyCode != null && _keyCode != KeyCode.None)
        {
            if (Input.GetKeyDown((KeyCode)_keyCode))
            {
                OnNextStep();
            }
        }
    }

    public void StartDialogue(string[] lines, KeyCode templateCode)
    {
        if (_Text == null || lines == null || lines.Length == 0) return;

        _dialogueLines = lines;
        _currentIndex = 0;
        _keyCode = templateCode;

        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.allLimit = true;
            PlayerController.Instance.StopAnim();
        }

        _animator.Play("On");
        _isChat = true;

        ShowLine(_currentIndex).Forget();
    }

    private void OnNextStep()
    {
        if (_isTyping)
        {
            // 타이핑 중일 때 키를 누르면 즉시 완성
            CompleteTyping();
        }
        else
        {
            // 타이핑이 이미 끝났다면 다음 문장으로
            GoToNextLine();
        }
    }

    private void CompleteTyping()
    {
        StopTyping();
        _Text.text = _currentFullText;
        _isTyping = false;

        // 키가 없을 경우, 글자가 완성된 직후부터 자동 넘김 카운트를 시작해야 합니다.
        if (_keyCode == null || _keyCode == KeyCode.None)
        {
            AutoNextStep().Forget();
        }
    }

    private async UniTaskVoid ShowLine(int index)
    {
        // 키가 있으면 {key} 치환, 없으면 빈 문자열로 처리
        string keyStr = (_keyCode == null || _keyCode == KeyCode.None) ? "" : _keyCode.ToString();
        _currentFullText = _dialogueLines[index].Replace("{key}", keyStr);

        await PlayTyping(_currentFullText);

        // 타이핑이 자연스럽게 끝났을 때(중간에 끊기지 않고)
        // 키 설정이 없다면 대기 후 자동으로 다음 단계 진행
        if (_isChat && (_keyCode == null || _keyCode == KeyCode.None))
        {
            await AutoNextStep();
        }
    }

    private async UniTask AutoNextStep()
    {
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delayNextChar), cancellationToken: cts.Token);
            if (_isChat && !_isTyping) // 대기 후 여전히 대화 중이고 타이핑 중이 아닐 때만
            {
                GoToNextLine();
            }
        }
        catch (OperationCanceledException) { /* 중도 스킵 시 무시 */ }
    }

    private void GoToNextLine()
    {
        _currentIndex++;
        if (_currentIndex < _dialogueLines.Length)
        {
            ShowLine(_currentIndex).Forget();
        }
        else
        {
            EndDialogue();
        }
    }

    private async UniTask PlayTyping(string fullText)
    {
        StopTyping();
        cts = new CancellationTokenSource();
        _isTyping = true;
        _Text.text = "";

        try
        {
            for (int i = 0; i < fullText.Length; i++)
            {
                _Text.text += fullText[i];
                await UniTask.Delay(TimeSpan.FromSeconds(delayPerChar), cancellationToken: cts.Token);
            }
            _isTyping = false;
        }
        catch (OperationCanceledException)
        {
            _isTyping = false;
        }
    }

    private void StopTyping()
    {
        if (cts != null)
        {
            cts.Cancel();
            cts.Dispose();
            cts = null;
        }
    }

    private void EndDialogue()
    {
        _isChat = false;
        _isTyping = false;
        _keyCode = null;
        _animator.Play("Off");
        if (PlayerController.Instance != null) PlayerController.Instance.allLimit = false;
        StopTyping();
    }

    private void OnDisable() => StopTyping();
}