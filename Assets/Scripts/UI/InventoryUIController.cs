using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUIController : MonoBehaviour
{
    [Header("Button Spawning")]
    public Transform inventoryButtonParent;
    public TMP_Text itemDescription;
    public GameObject buttonPrefab;
    private List<Button> activeButtons = new();

    [Header("Compartment Control")]
    public InventoryCompartmentController inventoryCompartmentController;
    private IReadOnlyList<InventoryItemData> activeCompartment;

    [Header("Context Menu")]
    [SerializeField] private GameObject contextMenu;
    private GameObject previousSelected; // ref store for returning to menu

    public static InventoryUIController instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = new InventoryUIController();
        }
        inventoryCompartmentController.OnCompartmentChanged += LoadInventory;
    }

    public void LoadInventory(int index)
    {
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

        ClearItems();
        PopulateInventory(activeCompartment);
    }

    private void ClearItems()
    {
        foreach (var button in activeButtons)
        {
            button.gameObject.GetComponent<InventoryItemButton>().OnSelected -= UpdateItemDescription;
            Destroy(button.gameObject);
        }
        activeButtons.Clear();
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
            activeButtons.Add(button.GetComponent<Button>());
        }
        activeButtons[0].Select();
    }

    public void UpdateItemDescription(InventoryItemData itemData)
    {
        itemDescription.text = itemData.itemDescription;
    }
    private void OpenContextMenu(GameObject button_)
    {
        inventoryCompartmentController.enabled = false;
        previousSelected = EventSystem.current.currentSelectedGameObject;
        previousSelected.GetComponent<InventoryItemButton>().enabled = false;
        contextMenu.SetActive(true);
        contextMenu.GetComponent<InventoryContextMenu>().OnMenuClose += ReturnToMenu;
        contextMenu.GetComponent<InventoryContextMenu>().InitializeContextMenu(button_.GetComponent<InventoryItemButton>().GetItemData());
        SetInventoryButtonStatus(false);
    }

    private void ReturnToMenu()
    {
        inventoryCompartmentController.enabled = true;
        previousSelected.GetComponent<InventoryItemButton>().enabled = true;
        EventSystem.current.SetSelectedGameObject(previousSelected);
        contextMenu.GetComponent<InventoryContextMenu>().OnMenuClose -= ReturnToMenu;
        contextMenu.SetActive(false);
        SetInventoryButtonStatus(true);

    }

    private void SetInventoryButtonStatus(bool status)
    {
        foreach(Button button in activeButtons)
        {
            button.interactable = status;
        }
    }
    
}
