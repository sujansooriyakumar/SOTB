using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIController : MonoBehaviour
{
    public Transform inventoryButtonParent;
    public TMP_Text itemDescription;
    public GameObject buttonPrefab;
    private List<Button> activeButtons = new();
    public static InventoryUIController instance;
    private IReadOnlyList<InventoryItemData> activeCompartment;

    public InventoryCompartmentController inventoryCompartmentController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        if(instance == null)
        {
            instance = new InventoryUIController();
        }
        inventoryCompartmentController.OnCompartmentChanged += LoadInventory;
    }
    void Start()
    {
    
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
            activeButtons.Add(button.GetComponent<Button>());
        }
        activeButtons[0].Select();
    }

    public void UpdateItemDescription(InventoryItemData itemData)
    {
        itemDescription.text = itemData.itemDescription;
    }
    
}
