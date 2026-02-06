using UnityEngine;

public class Sign : MonoBehaviour
{
    public enum Type { Question, Exclamation }
    public Type signType;
    
    public GameObject parent;

    private void Update()
    {
        if (parent == null)
        {
            gameObject.SetActive(false);
        }
    }
}
