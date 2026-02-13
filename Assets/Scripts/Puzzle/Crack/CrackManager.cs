using UnityEngine;

public class CrackManager : MonoBehaviour
{
    public static CrackManager Instance {get; private set;}

    public int crakLimit;
    public int currentLimit;

    private void Awake()
    {
        Instance = this;
    }
}
