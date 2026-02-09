using UnityEngine;

public class GetItem : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                ItemBase.Instance.AddItem();
            }
        }
    }
}
