using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Lists the inventory items that fit one equipment slot.
// Open it with:
//   UIController.Instance.Open<UIEquipmentDisplay>(m => m.SetEquipmentType(slot, OnItemChosen));
public class UIEquipmentDisplay : UIMenu
{
    [SerializeField] private GameObject itemPrefab;
    [Tooltip("Where item buttons are spawned, e.g. the Content object of a scroll view.")]
    [SerializeField] private Transform itemParent;

    private readonly List<Button> itemButtons = new();
    private EquipmentSlot slot;
    private int equipped;
    private Action<InventoryItemData> onItemChosen;
    public Action<int, EquipmentSlot> onItemSelected;
    // Called through Open's setup callback, before the menu opens.
    // The list itself is built in OnOpen.
    public void SetEquipmentType(EquipmentSlot equipmentSlot, int equipped_ ,Action<InventoryItemData> onChosen = null)
    {
        slot = equipmentSlot;
        onItemChosen = onChosen;
        equipped = equipped_;
    }

    protected override void OnOpen()
    {
        GameObject cancelButton = Instantiate(itemPrefab, itemParent);
        cancelButton.GetComponentInChildren<TMP_Text>().text = "Cancel";
        cancelButton.GetComponent<Button>().onClick.AddListener(() => UIController.Instance.Close(this));
        cancelButton.GetComponent<UIButton>().onSelect.AddListener(() => onItemSelected?.Invoke(equipped, slot));

        GameObject defaultObject = Instantiate(itemPrefab, itemParent);
        defaultObject.GetComponentInChildren<TMP_Text>().text = "---";
        defaultObject.GetComponent<UIButton>().onSelect.AddListener(() => onItemSelected?.Invoke(-1, slot));

        itemButtons.Add(cancelButton.GetComponent<Button>());
        itemButtons.Add(defaultObject.GetComponent<Button>());
        foreach (InventoryItemData item in GetItemsForSlot(slot))
        {
            ItemSO itemSO = InventoryController.Instance.GetItemData(item.uid);

            GameObject buttonObject = Instantiate(itemPrefab, itemParent);
            buttonObject.GetComponentInChildren<TMP_Text>().text = itemSO.itemName;

            if (buttonObject.TryGetComponent(out Button button))
            {
                InventoryItemData chosen = item;
                button.onClick.AddListener(() => onItemChosen?.Invoke(chosen));
                button.GetComponent<UIButton>().onSelect.AddListener(() => onItemSelected?.Invoke(chosen.uid, slot));
                itemButtons.Add(button);
            }
        }
        SetVerticalNavigation(itemButtons);
    }

    protected override void OnClose()
    {
        foreach (Button button in itemButtons)
        {
            // Deactivate first: Destroy is deferred to the end of the frame.
            button.gameObject.SetActive(false);
            Destroy(button.gameObject);
        }
        itemButtons.Clear();
        onItemChosen = null;
        onItemSelected = null;

    }

    // Focus() selects this when the menu opens, so the first item starts selected.
    protected override GameObject GetDefaultSelection() =>
        itemButtons.Count > 0 ? itemButtons[0].gameObject : null;

    private static List<InventoryItemData> GetItemsForSlot(EquipmentSlot slot)
    {
        IEnumerable<InventoryItemData> compartment = slot == EquipmentSlot.WEAPON
            ? InventoryController.Instance.weaponCompartmentList
            : InventoryController.Instance.armorCompartmentList;

        // A new list, so nothing here can modify the inventory's own lists.
        List<InventoryItemData> result = new();
        foreach (InventoryItemData item in compartment)
        {
            if (InventoryController.Instance.GetItemData(item.uid) is EquipmentItemSO equipment
                && equipment.equipmentSlot == slot)
            {
                result.Add(item);
            }
        }
        return result;
    }

    // Up/down between neighbours with wrap-around.
    private static void SetVerticalNavigation(List<Button> buttons)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].navigation = new Navigation
            {
                mode = Navigation.Mode.Explicit,
                selectOnUp = buttons[(i - 1 + buttons.Count) % buttons.Count],
                selectOnDown = buttons[(i + 1) % buttons.Count]
            };
        }
    }
}