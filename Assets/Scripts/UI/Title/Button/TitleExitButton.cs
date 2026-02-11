using UnityEngine;

public class TitleExitButton : MonoBehaviour
{
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject ChapterCollect;
    [SerializeField] private GameObject buttons;


    public void OnExit()
    {
        buttons.SetActive(!buttons.activeSelf);
        title.SetActive(!title.activeSelf);
        ChapterCollect.SetActive(!ChapterCollect.activeSelf);
    }
}
