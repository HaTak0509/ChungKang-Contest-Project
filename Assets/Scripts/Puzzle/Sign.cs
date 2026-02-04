using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
    public GameObject parent;

    private void Update()
    {
        if (parent == null)
        {
            gameObject.SetActive(false);
        }
    }
}
