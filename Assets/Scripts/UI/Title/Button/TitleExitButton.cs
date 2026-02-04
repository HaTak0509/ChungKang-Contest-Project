using UnityEngine;

public class TitleExitButton : MonoBehaviour
{
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject ChapterCollect;

    public void OnExit()
    {
        title.SetActive(!title.activeSelf);
        ChapterCollect.SetActive(!ChapterCollect.activeSelf);
    }
}
