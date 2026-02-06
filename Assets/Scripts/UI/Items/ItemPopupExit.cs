using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPopupExit : MonoBehaviour
{
    [SerializeField] private GameObject itemPopup;

    public void OnExit()
    {
        itemPopup.SetActive(false);
    }
}
