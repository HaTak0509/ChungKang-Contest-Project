using UnityEngine;

public class MouseSensitivityButton : MonoBehaviour
{
    public void OnMouseSensitivityButton()
    {
        SettingManager.Instance.Push(MenuType.Sensitivity);
        MenuController.Instance.Open(MenuType.Sensitivity);
    }
}
