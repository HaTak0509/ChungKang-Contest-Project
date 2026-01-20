using UnityEngine;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance;

    [Header("Menu Objects")]
    public GameObject menu;
    public GameObject sensitivity;
    public GameObject audio;

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
                menu.SetActive(true);
                break;
            case MenuType.Sensitivity:
                sensitivity.SetActive(true);
                break;
            case MenuType.Audio:
                audio.SetActive(true);
                break;
        }
    }

    private void CloseAll()
    {
        menu.SetActive(false);
        sensitivity.SetActive(false);
        audio.SetActive(false);
    }
}
