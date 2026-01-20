using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public void OnMenuButton()
    {
        SettingManager.Instance.Push(MenuType.Menu);
        MenuController.Instance.Open(MenuType.Menu);
    }
}
