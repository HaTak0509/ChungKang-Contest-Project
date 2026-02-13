using UnityEngine;

public class ExitApplication : MonoBehaviour
{
    public void OnExitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

}
