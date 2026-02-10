using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LiftPager : MonoBehaviour, IInteractable
{
    [SerializeField] private HorizontalLift horizontalLift;
    [SerializeField] private VerticalLift verticalLift;

    private bool _active;

    public void Interact()
    {
        if (_active)
        {
            if (horizontalLift != null && (!horizontalLift._isMoving))
            {
                horizontalLift.CallFromRemote();
            }
            else if (verticalLift != null && (!verticalLift.isMoving))
            {
                verticalLift.CallFromRemote();
            }
            else if (verticalLift != null && horizontalLift != null && (!verticalLift.isMoving))
            {
                horizontalLift.CallFromRemote();
                verticalLift.CallFromRemote();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _active = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _active = false;
        }
    }
}
