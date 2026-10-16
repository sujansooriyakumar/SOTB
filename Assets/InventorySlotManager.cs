using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotManager:MonoBehaviour
{
    public List<InventorySlot> inventorySlots;

    private void Awake()
    {
        foreach(InventorySlot slot in inventorySlots)
        {
            slot.OnDragItem += HandleDragItem;
            slot.OnDropItem += HandleDropItem;
        }
    }

    private void HandleDropItem(InventorySlot slot)
    {
        // use slot reference to replace current slot data 
    }

    private void HandleDragItem(InventorySlot slot)
    {
        // use slot reference to empty item
        // store reference to currently dragged item maybe with slot.GetSlotData
    }

}
