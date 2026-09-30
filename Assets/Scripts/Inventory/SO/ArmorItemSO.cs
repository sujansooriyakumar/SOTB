using UnityEngine;

[CreateAssetMenu(fileName = "ArmorItemSO", menuName = "SOTB/Inventory/ArmorItemSO")]
public class ArmorItemSO : SellableItemSO
{
    public int armorRating;
    public ArmorSlot armorSlot;

    public override InventoryItemData CreateRuntimeData()
    {
        return new ArmorItemData(this);
    }
}
