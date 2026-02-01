using UnityEngine;

public class ObjectConfinement : MonoBehaviour
{
    private GameObject stuckObject;

    private void OnDisable()
    {
        ReleaseObject();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("PuzzleObject") &&
            !collision.CompareTag("Enemy"))
            return;

        if (stuckObject != null && !stuckObject.activeSelf)
        {
            stuckObject = collision.gameObject;
            return;
        }

        if (stuckObject != null) return;

        stuckObject = collision.gameObject;

        if (stuckObject.TryGetComponent<WarpingInterface>(out var warpInteract))
            warpInteract.Warping();
    }

    private void ReleaseObject()
    {
        if (stuckObject != null &&
            stuckObject.TryGetComponent<WarpingInterface>(out var warpInteract))
            warpInteract.Warping();

        stuckObject = null;
    }
}
