using UnityEngine;

public class ObjectConfinement : MonoBehaviour
{
    private GameObject stuckObject;
    private SpriteRenderer objectSp;

    private void OnDisable()
    {
        ReleaseObject();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PuzzleObject") || collision.gameObject.CompareTag("Enemy"))
        {
            if (!stuckObject.gameObject.activeSelf && stuckObject != null) stuckObject = null;
            if (stuckObject != null) return;

            stuckObject = collision.gameObject;
            objectSp = stuckObject.GetComponent<SpriteRenderer>();

            objectSp.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

            if (stuckObject.TryGetComponent<WarpingInterface>(out var warpInteract))
            {
                warpInteract.Warping();
            }
        }
    }

    private void ReleaseObject()
    {
        if (objectSp != null)
            objectSp.maskInteraction = SpriteMaskInteraction.None;

        if (stuckObject != null &&
            stuckObject.TryGetComponent<WarpingInterface>(out var warpInteract))
            warpInteract.Warping();

        objectSp = null;
        stuckObject = null;
    }
}
