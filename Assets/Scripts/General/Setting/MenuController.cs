using UnityEngine;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance;

    [Header("Menu Objects")]
    public GameObject menuButton;
    public GameObject resetButton;
    public GameObject audioButton;
    public GameObject sensitivityButton;
    public GameObject returnButton;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Open(MenuType type)
    {
        CloseAll();

        switch (type)
        {
            case MenuType.Menu:
                menuButton.SetActive(true);
                break;
            case MenuType.Sensitivity:
                sensitivityButton.SetActive(true);
                break;
            case MenuType.Audio:
                audioButton.SetActive(true);
                break;
        }
    }

    private void CloseAll()
    {
        menuButton.SetActive(false);
        resetButton.SetActive(false);
        audioButton.SetActive(false);
        sensitivityButton.SetActive(false);
        returnButton.SetActive(false);
    }
}
