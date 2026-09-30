using System;
using UnityEngine;

[Serializable]

public class SellableItemData : InventoryItemData
{
    public int sellPrice;
    public SellableItemData(SellableItemSO config_) : base(config_)
    {
        sellPrice = config_.sellPrice;
        itemName = config_.itemName;
        itemDescription = config_.itemDescription;
        uid = config_.uID;
        qty = 1;
    }
}
