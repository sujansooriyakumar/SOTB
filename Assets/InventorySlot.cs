using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDragHandler, IDropHandler
{
    private InventorySlotData _slotData;
    public event Action<InventorySlot> OnDragItem;
    public event Action<InventorySlot> OnDropItem;
    public void OnDrag(PointerEventData eventData)
    {
        OnDragItem?.Invoke(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnDropItem?.Invoke(this);
    }

    public InventorySlotData GetSlotData() { return  _slotData; }
}

public struct InventorySlotData
{
    // item data goes here
}
