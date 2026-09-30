using System;
using UnityEngine;

[Serializable]

public class ArmorItemData : SellableItemData
{
    public int armorRating;
    public ArmorSlot armorSlot;
    public ArmorItemData(ArmorItemSO config_) : base(config_)
    {
        armorRating = config_.armorRating;
        armorSlot = config_.armorSlot;
        qty = 1;
        uid = config_.uID;
        itemDescription = config_.itemDescription;
        itemName = config_.itemName;
        sellPrice = config_.sellPrice;
    }
}
