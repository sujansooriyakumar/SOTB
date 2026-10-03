using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventoryMenu : UIMenu
{
    [Header("Button Spawning")]
    public Transform inventoryButtonParent;
    public TMP_Text itemDescription;
    public GameObject buttonPrefab;
   // private List<Button> activeButtons = new();

    [Header("Compartment Control")]
    public InventoryCompartmentController inventoryCompartmentController;
    private IReadOnlyList<InventoryItemData> activeCompartment;
    private int compartmentIndex = -1;

    [Header("Context Menu")]
    [SerializeField] private GameObject contextMenu;
    private GameObject previousSelected; // ref store for returning to menu

    public override void ActivateMenu()
    {
        base.ActivateMenu();
        inventoryCompartmentController.OnCompartmentChanged += LoadInventory;
        int initIndex = 0;
        if (compartmentIndex > 0) initIndex = compartmentIndex;
        inventoryCompartmentController.SetActiveCompartment(initIndex);
        SetInventoryButtonStatus(true);
        inventoryCompartmentController.enabled = true;
        if(lastSelected != null) lastSelected.GetComponent<InventoryItemButton>().enabled = true;



    }
    public override void DeactivateMenu()
    {
        base.DeactivateMenu();
        inventoryCompartmentController.OnCompartmentChanged -= LoadInventory;
        SetInventoryButtonStatus(false);

    }

    public override void CloseMenu()
    {
        compartmentIndex = -1;
        lastSelected = null;
        base.CloseMenu();
    }


    public void LoadInventory(int index)
    {
        if (index == compartmentIndex) return;
        switch (index)
        {
            case 0:
                activeCompartment = InventoryController.Instance.consumableCompartmentList;
                break;
            case 1:
                activeCompartment = InventoryController.Instance.weaponCompartmentList;
                break;
            case 2:
                activeCompartment = InventoryController.Instance.armorCompartmentList;
                break;
            case 3:
                activeCompartment = InventoryController.Instance.questCompartmentList;
                break;
        }
        compartmentIndex = index;
        ClearItems();
        PopulateInventory(activeCompartment);
    }

    private void ClearItems()
    {
        foreach (var button in buttons)
        {
            button.gameObject.GetComponent<InventoryItemButton>().OnSelected -= UpdateItemDescription;
            Destroy(button.gameObject);
        }
        buttons.Clear();
    }

    public void PopulateInventory(IReadOnlyList<InventoryItemData> data)
    {
        if (data.Count == 0) return;
        foreach (InventoryItemData item in data)
        {
            GameObject button = Instantiate(buttonPrefab, inventoryButtonParent);
            InventoryItemButton buttonData = button.GetComponent<InventoryItemButton>();
            buttonData.InitializeButton(item);
            buttonData.OnSelected += UpdateItemDescription;
            button.GetComponent<Button>().onClick.AddListener(() => OpenContextMenu(button));
            buttons.Add(button.GetComponent<Button>());
        }
        buttons[0].Select();
    }

    public void UpdateItemDescription(InventoryItemData itemData)
    {
        itemDescription.text = InventoryController.Instance.GetItemData(itemData.uid).itemDescription;
    }
    private void OpenContextMenu(GameObject button_)
    {
        inventoryCompartmentController.enabled = false;
        lastSelected = EventSystem.current.currentSelectedGameObject;
        lastSelected.GetComponent<InventoryItemButton>().enabled = false;
        UIController.Instance.OpenMenu(2);
       // contextMenu.SetActive(true);
       // contextMenu.GetComponent<InventoryContextMenu>().OnMenuClose += ReturnToMenu;
      //  contextMenu.GetComponent<InventoryContextMenu>().GetItemSelected(button_.GetComponent<InventoryItemButton>().GetItemData());
        SetInventoryButtonStatus(false);
    }
 
    private void SetInventoryButtonStatus(bool status)
    {
        foreach (Button button in buttons)
        {
            button.interactable = status;
        }
    }

  
}
