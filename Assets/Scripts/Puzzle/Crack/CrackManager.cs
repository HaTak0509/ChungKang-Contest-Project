using UnityEngine;

public class CrackManager : MonoBehaviour
{
    public static CrackManager Instance {get; private set;}

    public int crakLimit;
    public int currentLimit;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}
