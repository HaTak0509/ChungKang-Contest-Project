using UnityEngine;

public class ChapterCollectButton : MonoBehaviour
{
    [SerializeField] GameObject title;
    [SerializeField] GameObject chaptercollect;
    [SerializeField] GameObject exit;

    public void OnchapterChange()
    {
        title.SetActive(!title.activeSelf);
        chaptercollect.SetActive(!chaptercollect.activeSelf);
        exit.SetActive(!exit.activeSelf);
    }
}
