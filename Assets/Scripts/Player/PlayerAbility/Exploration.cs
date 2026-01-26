using UnityEngine;

public class Exploration : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Crack"))
        {
            Crack crack = collision.gameObject.GetComponent<Crack>();

            if (!crack.activationCrackActive)
                crack.deactivationCrackActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Crack"))
        {
            Crack crack = collision.gameObject.GetComponent<Crack>();
            
            if (!crack.activationCrackActive)
                crack.deactivationCrackActive = false;
        }
    }
}
