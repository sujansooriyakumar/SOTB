using System;
using UnityEngine;

[Serializable]

public class ConsumableItemData : SellableItemData
{
    public ConsumableItemData(ConsumableItemSO config_) : base(config_)
    {
        itemName = config_.itemName;
        itemDescription = config_.itemDescription;
        uid = config_.uID;
        qty = 1;
        sellPrice = config_.sellPrice;
    }
}
