using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private BoxCollider2D _collider;
    private bool _input;

    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (_input)
        {
            _collider.isTrigger = true;
        }
        else 
        {
            _collider.isTrigger = false;
        }
    }
}
