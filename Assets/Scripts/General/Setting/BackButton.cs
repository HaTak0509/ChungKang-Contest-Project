using UnityEngine;

public class BackButton : MonoBehaviour
{
    public static BackButton Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OnBack();
    }

    public void OnBack()
    {
        if (SettingManager.Instance.Count <= 1)
            return;

        // 현재 메뉴 제거
        SettingManager.Instance.Pop();

        // 이전 메뉴 열기
        MenuType prev = SettingManager.Instance.Peek();
        MenuController.Instance.Open(prev);
    }
}
