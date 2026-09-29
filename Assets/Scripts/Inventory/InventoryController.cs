using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private ItemDatabaseSO itemDatabase;
    private Dictionary<int, ItemSO> itemDictionary = new();

    private Dictionary<int, InventoryItemData> consumableCompartment = new();
    private Dictionary<int, InventoryItemData> weaponCompartment = new();
    private Dictionary<int, InventoryItemData> armorCompartment = new();
    private Dictionary<int, InventoryItemData> questCompartment = new();


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
        InitializeDatabase();

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
        Dictionary<int, InventoryItemData> targetDictionary = GetTargetInventory(uid);

        if (!targetDictionary.TryAdd(uid, new InventoryItemData(GetItemData(uid))))
        {
            targetDictionary[uid].qty += 1;
        }
        UpdateListFromDictionary(targetDictionary);
    }

    public void RemoveItem(int uid)
    {
        Dictionary<int, InventoryItemData> targetDictionary = GetTargetInventory(uid);

        if (targetDictionary.ContainsKey(uid))
        {
            targetDictionary[uid].qty -= 1;
            if (targetDictionary[uid].qty <= 0)
            {
                targetDictionary.Remove(uid);
            }
        }
        UpdateListFromDictionary(targetDictionary);
    }

    public List<InventoryItemData> GetInventoryList(Dictionary<int, InventoryItemData> targetDictionary)
    {
        List<InventoryItemData> inventoryList = consumableCompartmentList;
        if (targetDictionary == weaponCompartment) inventoryList = weaponCompartmentList;
        if(targetDictionary == armorCompartment) inventoryList = armorCompartmentList;
        if(targetDictionary == questCompartment) inventoryList = questCompartmentList;

        return inventoryList;
    }

    public Dictionary<int, InventoryItemData> GetTargetInventory(int uid)
    {
        Dictionary<int, InventoryItemData> targetDictionary = consumableCompartment;

        ItemSO targetItem = itemDictionary[uid];

        if (targetItem is WeaponItemSO weaponItem)
        {
            targetDictionary = weaponCompartment;
        }

        if (targetItem is ArmorItemSO armorItem)
        {
            targetDictionary = armorCompartment;
        }

        if (targetItem is QuestItemSO questItem)
        {
            targetDictionary = questCompartment;
        }

        return targetDictionary;
    }
    private void UpdateListFromDictionary(Dictionary<int, InventoryItemData> targetInventory)
    {
        List<InventoryItemData> listToUpdate = GetInventoryList(targetInventory);
        listToUpdate.Clear();
        foreach (KeyValuePair<int, InventoryItemData> kvp in targetInventory)
        {
            listToUpdate.Add(kvp.Value);
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

[Serializable]
public class InventoryItemData
{
    public int uid;
    public int qty;
    public string itemName;
    public string itemDescription;
    public InventoryItemData(ItemSO config_)
        {
        uid = config_.uID;
        qty = 1;
        itemName = config_.itemName;
        itemDescription = config_.itemDescription;
        }

    }

