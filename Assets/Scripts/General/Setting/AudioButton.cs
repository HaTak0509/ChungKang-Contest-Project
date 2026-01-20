using UnityEngine;

public class AudioButton : MonoBehaviour
{
    public void OnAudioButton()
    {
        SettingManager.Instance.Push(MenuType.Audio);
        MenuController.Instance.Open(MenuType.Audio);
    }
}
