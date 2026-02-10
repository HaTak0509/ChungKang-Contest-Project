using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageListUI : MonoBehaviour
{
    [SerializeField] GameObject stageButtonPrefab;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] StageData stageData;
    [SerializeField] Transform content;
    [SerializeField] int stage;

    void Start()
    {
        scrollRect.verticalNormalizedPosition = 1f;

        for (int i = 1; i <= stage; i++)
        {
            GameObject obj = Instantiate(stageButtonPrefab, content);

            Button btn = obj.GetComponent<Button>();
            TMP_Text text = obj.GetComponentInChildren<TMP_Text>();

            text.text = stageData.stageName;

            int index = i;
            btn.onClick.AddListener(() =>{StageCollect.CollectStage(index); });
        }
    }
}
