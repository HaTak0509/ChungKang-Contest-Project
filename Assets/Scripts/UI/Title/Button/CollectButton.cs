using UnityEngine;
using UnityEngine.UI;

public class CollectButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button.onClick.AddListener(() => Collect());
    }

    private void Collect()
    {

    }
}
