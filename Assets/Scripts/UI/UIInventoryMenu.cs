using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventoryMenu : UIMenu
{
    [Header("Button Spawning")]
    [SerializeField] private Transform inventoryButtonParent;
    [SerializeField] private TMP_Text itemDescription;
    [SerializeField] private GameObject buttonPrefab;

    [Header("Compartment Control")]
    [SerializeField] private InventoryCompartmentController inventoryCompartmentController;

    private readonly List<InventoryItemButton> itemButtons = new();
    private IReadOnlyList<InventoryItemData> activeCompartment;
    private int compartmentIndex = -1;
    private int selectedIndex;

    protected override void OnOpen()
    {

        inventoryCompartmentController.OnCompartmentChanged += LoadInventory;
        InventoryController.Instance.OnInventoryUpdated += Refresh;

        compartmentIndex = -1;
        inventoryCompartmentController.SetActiveCompartment(0);
    }

    protected override void OnFocus()
    {
        foreach (InventoryItemButton button in itemButtons)
        {
            button.enabled = true;
        }
    }

    protected override void OnUnfocus()
    {

        if (LastSelected != null && LastSelected.TryGetComponent(out InventoryItemButton button))
        {
            button.enabled = false;
        }
    }

    protected override void OnClose()
    {
        inventoryCompartmentController.OnCompartmentChanged -= LoadInventory;
        InventoryController.Instance.OnInventoryUpdated -= Refresh;

        ClearItems();
        activeCompartment = null;
        compartmentIndex = -1;
    }

    public override bool HandlePage(int direction)
    {
        inventoryCompartmentController.Step(direction);
        return true;
    }

    protected override GameObject GetDefaultSelection()
    {
        if (itemButtons.Count == 0) return null;

        int index = Mathf.Clamp(selectedIndex, 0, itemButtons.Count - 1);
        return itemButtons[index].gameObject;
    }

    private void LoadInventory(int index)
    {
        if (index == compartmentIndex) return;

        activeCompartment = index switch
        {
            0 => InventoryController.Instance.consumableCompartmentList,
            1 => InventoryController.Instance.weaponCompartmentList,
            2 => InventoryController.Instance.armorCompartmentList,
            3 => InventoryController.Instance.questCompartmentList,
            _ => activeCompartment
        };

        compartmentIndex = index;
        selectedIndex = 0;
        Rebuild();
    }

    private void Refresh()
    {
        if (activeCompartment != null) Rebuild();
    }

    private void Rebuild()
    {
        ClearItems();
        PopulateInventory(activeCompartment);

        if (itemButtons.Count == 0) itemDescription.text = "";

        if (IsFocused)
        {
            EventSystem.current.SetSelectedGameObject(GetDefaultSelection());
        }
    }

    private void ClearItems()
    {
        foreach (InventoryItemButton button in itemButtons)
        {

            button.gameObject.SetActive(false);
            Destroy(button.gameObject);
        }
        itemButtons.Clear();
    }

    private void PopulateInventory(IReadOnlyList<InventoryItemData> data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            int index = i;
            InventoryItemData item = data[i];

            GameObject buttonObject = Instantiate(buttonPrefab, inventoryButtonParent);
            InventoryItemButton itemButton = buttonObject.GetComponent<InventoryItemButton>();
            itemButton.InitializeButton(item);

            itemButton.OnSelected += selected => OnItemSelected(index, selected);
            buttonObject.GetComponent<Button>().onClick.AddListener(() => OpenContextMenu(item));

            itemButtons.Add(itemButton);
        }
    }

    private void OnItemSelected(int index, InventoryItemData itemData)
    {
        selectedIndex = index;
        itemDescription.text = InventoryController.Instance.GetItemData(itemData.uid).itemDescription;
    }

    private void OpenContextMenu(InventoryItemData item)
    {
        UIController.Instance.Open<UIItemContextMenu>(menu => menu.SetItem(item));
    }
}