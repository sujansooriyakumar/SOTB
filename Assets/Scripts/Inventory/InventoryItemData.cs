using System;
using UnityEngine;

[Serializable]
public class InventoryItemData
{
    public int uid;
    public int qty;
    public string itemName;
    public string itemDescription;
    public InventoryItemData(ItemSO config_)
    {
        uid = config_.uID;
        qty = 1;
        itemName = config_.itemName;
        itemDescription = config_.itemDescription;
    }

}

