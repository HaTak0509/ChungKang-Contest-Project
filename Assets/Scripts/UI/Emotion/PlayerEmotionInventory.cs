using System;
using UnityEngine;

public class PlayerEmotionInventory : MonoBehaviour
{
    public int InventoryCount = 3;


    public Transform Inventory {  get; private set; }

    public static PlayerEmotionInventory Instance;
    public static Action<bool> OnPannel;
    public static Action<string> OnErrorPannel;



    private void Start()
    {
        Instance = this;
        Inventory = gameObject.transform;
    }

}
