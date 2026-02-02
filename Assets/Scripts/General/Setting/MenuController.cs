using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject menuButton;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject ResetButton;
    [SerializeField] private GameObject ReturnMainMenuButton;

    public void OnMenu()
    {
        menu.SetActive(true);
        menuButton.SetActive(false);
    }

    public void OnBack()
    {
        menu.SetActive(false);
        menuButton.SetActive(true);
    }

    public void OnReset()
    {
        LevelReset.Instance.OnReset();
    }

    public void OnReturnMainMenu()
    {
        LevelReset.Instance.LoadLevel(0);
    }
}
