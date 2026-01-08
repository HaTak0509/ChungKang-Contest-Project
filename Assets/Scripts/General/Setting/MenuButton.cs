using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private GameObject menuImage;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnMenuButton();
        }
    }

    public void OnMenuButton()
    {
        bool isActive = menuImage.activeSelf;
        menuImage.SetActive(!isActive);
    }
}
