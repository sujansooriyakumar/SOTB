using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableItemSO", menuName = "SOTB/Inventory/ConsumableItemSO")]
public class ConsumableItemSO : SellableItemSO
{
    public override InventoryItemData CreateRuntimeData()
    {
        return new ConsumableItemData(this);
    }
}
