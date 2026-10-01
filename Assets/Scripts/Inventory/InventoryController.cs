using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private ItemDatabaseSO itemDatabase;
    private Dictionary<int, ItemSO> itemDictionary = new();


    private Dictionary<int, int> inventory = new();

    public List<InventoryItemData> weaponCompartmentList = new();
    public List<InventoryItemData> armorCompartmentList = new();
    public List<InventoryItemData> consumableCompartmentList = new();
    public List<InventoryItemData> questCompartmentList = new();

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
        //InitializeDatabase();
        //AddItem(0);
        //AddItem(42);

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

        if (!inventory.TryAdd(uid, 1))
        {
            inventory[uid] += 1;
        }
        UpdateListFromDictionary();
    }

    public void RemoveItem(int uid)
    {
       // Dictionary<int, InventoryItemData> targetDictionary = GetTargetInventory(uid);

        if (inventory.ContainsKey(uid))
        {
            inventory[uid] -= 1;
            if (inventory[uid] <= 0)
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
        foreach (KeyValuePair<int, int> kvp in inventory)
        {
            if(GetItemData(kvp.Value) is ConsumableItemSO consumable)
            {
                consumableCompartmentList.Add(new InventoryItemData(kvp));
            }

            if(GetItemData(kvp.Value) is WeaponItemSO weapon)
            {
                weaponCompartmentList.Add(new InventoryItemData(kvp));
            }

            if(GetItemData(kvp.Value) is ArmorItemSO armor)
            {
                armorCompartmentList.Add(new InventoryItemData(kvp));
            }

            if(GetItemData(kvp.Value) is QuestItemSO quest)
            {
                questCompartmentList.Add(new InventoryItemData(kvp));
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

