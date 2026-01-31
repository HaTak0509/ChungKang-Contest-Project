using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectConfinement : MonoBehaviour
{
    private GameObject stuckObject;
    private SpriteRenderer objectSp;

    private void OnDisable()
    {
        objectSp.maskInteraction = SpriteMaskInteraction.None;
        objectSp = null;
        stuckObject = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PuzzleObject") || collision.gameObject.CompareTag("Enemy"))
        {
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
}
