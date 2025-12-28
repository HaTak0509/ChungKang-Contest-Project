using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushing : MonoBehaviour
{
    private PushingObject _pushingOb;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PushingObject>() != null)
        {
            _pushingOb = collision.gameObject.GetComponent<PushingObject>();
        }
    }

    private void Update()
    {
        if (_pushingOb != null)
        {

        }
    }
}
