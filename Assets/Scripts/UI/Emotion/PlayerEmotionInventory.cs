using System;
using UnityEngine;

public class PlayerEmotionInventory : MonoBehaviour
{
    public Transform Inventory {  get; private set; }

    public static PlayerEmotionInventory Instance;
    public static Action<bool> OnPannel;

    public int inventorySize;

    private void Start()
    {
        Instance = this;
        Inventory = gameObject.transform;
    }

}
