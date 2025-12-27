using System;
using UnityEngine;

public class PlayerEmotionInventory : MonoBehaviour
{
    [Range(1, 3)] public int InventoryCount = 3;

    public Transform Inventory {  get; private set; }
    public GameObject EmotionPrefab;

    public static PlayerEmotionInventory Instance;
    public static Action<bool> OnPannel;
    public static Action<string> OnErrorPannel;



    private void Start()
    {
        Instance = this;
        Inventory = gameObject.transform;

        GetComponent<RectTransform>().sizeDelta = new Vector2(60, InventoryCount * 50f);
    }

}
