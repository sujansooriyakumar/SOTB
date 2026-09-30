using System;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemQuantity;
    [SerializeField] private Image selected;
    private InventoryItemData itemRef;
    public event Action<InventoryItemData> OnSelected;


    public void InitializeButton(InventoryItemData item)
    {
        itemRef = item;
        itemName.text = item.itemName;
        itemQuantity.text = "x" + item.qty.ToString();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selected.enabled = false;
    }

    public void OnSelect(BaseEventData eventData)
    {
        selected.enabled = true;
        OnSelected?.Invoke(itemRef);

    }
}
