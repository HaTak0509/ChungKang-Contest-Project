using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageListUI : MonoBehaviour
{
    [SerializeField] GameObject _stageButtonPrefab;
    [SerializeField] ScrollRect _scrollRect;
    [SerializeField] Transform _content;
    [SerializeField] int stage;

    void Start()
    {
        _scrollRect.verticalNormalizedPosition = 1f;

        for (int i = 1; i <= stage; i++)
        {
            GameObject obj = Instantiate(_stageButtonPrefab, _content);

            Button btn = obj.GetComponent<Button>();
            TMP_Text text = obj.GetComponentInChildren<TMP_Text>();

            text.text = $"튜토리얼 {i}";

            int index = i;
            btn.onClick.AddListener(() =>
            {
                Debug.Log($"Stage {index} 클릭");
            });
        }
    }
}
