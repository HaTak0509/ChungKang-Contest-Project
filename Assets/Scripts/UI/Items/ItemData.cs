using UnityEngine;

[CreateAssetMenu(menuName = "Item/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemID;
    public Sprite icon;
    public string itemName;

    [TextArea(3, 10)]
    public string description;
}
