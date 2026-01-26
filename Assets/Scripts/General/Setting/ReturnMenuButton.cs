using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnMenuButton : MonoBehaviour
{
    public static ReturnMenuButton Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}
