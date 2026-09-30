using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private ItemDatabaseSO itemDatabase;
    private Dictionary<int, ItemSO> itemDictionary = new();


    private Dictionary<int, InventoryItemData> inventory = new();

    public List<WeaponItemData> weaponCompartmentList = new();
    public List<ArmorItemData> armorCompartmentList = new();
    public List<ConsumableItemData> consumableCompartmentList = new();
    public List<QuestItemData> questCompartmentList = new();

    public static InventoryController Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        InitializeDatabase();
        AddItem(0);
        AddItem(42);

    }

    public ItemSO GetItemData(int uid)
    {
        return itemDictionary[uid];
    }
    public void InitializeDatabase()
    {
        itemDictionary = itemDatabase.InitializeItemDictionary();
    }

    public void AddItem(int uid)
    {
       // Dictionary<int, InventoryItemData> targetDictionary = GetTargetInventory(uid);

        if (!inventory.TryAdd(uid, GetItemData(uid).CreateRuntimeData()))
        {
            inventory[uid].qty += 1;
        }
        UpdateListFromDictionary();
    }

    public void RemoveItem(int uid)
    {
       // Dictionary<int, InventoryItemData> targetDictionary = GetTargetInventory(uid);

        if (inventory.ContainsKey(uid))
        {
            inventory[uid].qty -= 1;
            if (inventory[uid].qty <= 0)
            {
                inventory.Remove(uid);
            }
        }
        UpdateListFromDictionary();
    }



    
    private void UpdateListFromDictionary()
    {
        weaponCompartmentList.Clear();
        armorCompartmentList.Clear();
        consumableCompartmentList.Clear();
        questCompartmentList.Clear();
        foreach (KeyValuePair<int, InventoryItemData> kvp in inventory)
        {
            if(kvp.Value is ConsumableItemData consumable)
            {
                consumableCompartmentList.Add(consumable);
            }

            if(kvp.Value is WeaponItemData weapon)
            {
                weaponCompartmentList.Add(weapon);
            }

            if(kvp.Value is ArmorItemData armor)
            {
                armorCompartmentList.Add(armor);
            }

            if(kvp.Value is QuestItemData quest)
            {
                questCompartmentList.Add(quest);
            }
        }
    }

    public void ReorderItems(List<InventoryItemData> inventoryCompartment, int fromIndex, int toIndex)
    {
        InventoryItemData fromItem = inventoryCompartment[fromIndex];
        InventoryItemData toItem = inventoryCompartment[toIndex];

        inventoryCompartment[toIndex] = fromItem;
        inventoryCompartment[fromIndex] = toItem;
    }
}

