using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBox : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCol;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (boxCol.enabled)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }
}
