using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LiftPager : MonoBehaviour
{
    [SerializeField] private HorizontalLift horizontalLift;
    [SerializeField] private VerticalLift verticalLift;

    private bool _active;

    private void Update()
    {
        if (_active)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if(horizontalLift != null && (!horizontalLift._calling && !horizontalLift._isMoving))
                {
                    horizontalLift._calling = true;
                }
                else if (verticalLift != null && (!verticalLift._calling && !verticalLift._isMoving))
                {
                    verticalLift._calling = true;
                }
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
