using UnityEngine;

public class Exploration : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Crack")) return;

        Crack crack = collision.GetComponent<Crack>();
        if (crack != null)
        {
            crack.SetPlayerInRange(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Crack")) return;

        Crack crack = collision.GetComponent<Crack>();
        if (crack != null)
        {
            crack.SetPlayerInRange(false);
        }
    }
}
