using UnityEngine;

public class TitleExitButton : MonoBehaviour
{
    [SerializeField] private GameObject firstScene;
    [SerializeField] private GameObject ChapterCollect;
    [SerializeField] private GameObject title;

    public void OnExit()
    {
        title.SetActive(true);
        firstScene.SetActive(!firstScene.activeSelf);
        ChapterCollect.SetActive(!ChapterCollect.activeSelf);
    }
}
