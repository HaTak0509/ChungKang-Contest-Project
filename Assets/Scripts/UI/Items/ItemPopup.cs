using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPopup : MonoBehaviour
{
    [SerializeField] private GameObject itemPopup;

    private void Update()
    {
        if (itemPopup == null) return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            itemPopup.SetActive(!itemPopup.activeSelf);
        }
    }
}
