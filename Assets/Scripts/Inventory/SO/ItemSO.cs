using UnityEngine;

public abstract class ItemSO : ScriptableObject
{
    public int uID;
    public string itemName;
    public string itemDescription;

    public abstract InventoryItemData CreateRuntimeData();
}
