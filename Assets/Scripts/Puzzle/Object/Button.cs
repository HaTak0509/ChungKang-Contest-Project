using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private Door targetDoor;

    private bool active;

    void Update()
    {
        if (active)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (targetDoor.currentState)
                {
                    targetDoor.CloseDoor();
                }
                else
                {
                    targetDoor.OpenDoor();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            active = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            active = false;
        }
    }
}
