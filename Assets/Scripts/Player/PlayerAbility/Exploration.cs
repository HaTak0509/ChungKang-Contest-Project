using UnityEngine;

public class Exploration : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Crack") && collision.gameObject.layer != LayerMask.NameToLayer("Crack")) return;

        Crack crack = collision.GetComponent<Crack>();
        if (crack != null)
        {
            crack.SetPlayerInRange(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Crack") && collision.gameObject.layer != LayerMask.NameToLayer("Crack")) return;

        Crack crack = collision.GetComponent<Crack>();
        if (crack != null)
        {
            crack.SetPlayerInRange(false);
        }
    }
    private void OnEnable()
    {
        SoundManager.Instance.PlaySFX("player_detaction", SoundManager.SoundOutput.SFX, 1);
    }
}
