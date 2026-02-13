using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject menuButton;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject ResetButton;
    [SerializeField] private GameObject ReturnMainMenuButton;

    private bool _IsOpen = false;   
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {

            _IsOpen = !_IsOpen;

            if (_IsOpen)
            {
                OnMenu();
            }
            else
            {
                OnBack();
            }

        }   
    }

    public void OnMenu()
    {
        _IsOpen = true;
        menu.SetActive(true);
        if (menuButton != null)
        {
            menuButton.SetActive(false);
        }
    }

    public void OnReset()
    {
        LevelManager.Instance.OnReset();
        OnBack();
    }

    public void OnBack()
    {
        _IsOpen = false;
        menu.SetActive(false);
        if (menuButton  != null)
        {
            menuButton.SetActive(true);
        }
    }


    public void OnReturnMainMenu()
    {
        LevelManager.Instance.LoadLevel(0);
    }

    public void OnButtonSound()
    {
        SoundManager.Instance.PlaySFX("UI", SoundManager.SoundOutput.UI);
    }
}
