using UnityEngine;

public class SellableItemSO:ItemSO
{
    public int sellPrice;

    public override InventoryItemData CreateRuntimeData()
    {
        return new SellableItemData(this);
    }
}
