using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private ItemDatabaseSO itemDatabase;
    private Dictionary<int, ItemSO> itemDictionary = new();

    private Dictionary<int, int> consumableCompartment = new();
    private Dictionary<int, int> weaponCompartment = new();
    private Dictionary<int, int> armorCompartment = new();
    private Dictionary<int, int> questCompartment = new();


    public List<InventoryItemData> weaponInventoryList = new();
    public List<InventoryItemData> armorInventoryList = new();
    public List<InventoryItemData> consumableInventoryList = new();
    public List<InventoryItemData> questInventoryList = new();




    private void Start()
    {
        InitializeDatabase();
        for (int i = 0; i < 50; i++)
        {
            AddItem(0);
        }
        for(int i = 0; i < 50; i++)
        {
            AddItem(1);
        }


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
        Dictionary<int, int> targetDictionary = GetTargetInventory(uid);

        if (!targetDictionary.TryAdd(uid, 1))
        {
            targetDictionary[uid] += 1;
        }
        UpdateListFromDictionary(targetDictionary);
    }

    public void RemoveItem(int uid)
    {
        Dictionary<int, int> targetDictionary = GetTargetInventory(uid);

        if (targetDictionary.ContainsKey(uid))
        {
            targetDictionary[uid] -= 1;
            if (targetDictionary[uid] <= 0)
            {
                targetDictionary.Remove(uid);
            }
        }
        UpdateListFromDictionary(targetDictionary);
    }

    public List<InventoryItemData> GetInventoryList(Dictionary<int, int> targetDictionary)
    {
        List<InventoryItemData> inventoryList = consumableInventoryList;
        if (targetDictionary == weaponCompartment) inventoryList = weaponInventoryList;
        if(targetDictionary == armorCompartment) inventoryList = armorInventoryList;
        if(targetDictionary == questCompartment) inventoryList = questInventoryList;
        if(targetDictionary == consumableCompartment) inventoryList = consumableInventoryList;

        return inventoryList;
    }

    public Dictionary<int, int> GetTargetInventory(int uid)
    {
        Dictionary<int, int> targetDictionary = questCompartment;

        ItemSO targetItem = itemDictionary[uid];

        if (targetItem is WeaponItemSO weaponItem)
        {
            targetDictionary = weaponCompartment;
        }

        if (targetItem is ConsumableItemSO consumableItem)
        {
            targetDictionary = consumableCompartment;
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
    private void UpdateListFromDictionary(Dictionary<int, int> targetInventory)
    {
        List<InventoryItemData> listToUpdate = GetInventoryList(targetInventory);
        listToUpdate.Clear();
        foreach (KeyValuePair<int, int> kvp in targetInventory)
        {
            listToUpdate.Add(new InventoryItemData(kvp.Key, kvp.Value));
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

    public InventoryItemData(int uid, int qty)
        {
            this.uid = uid;
            this.qty = qty;
        }
        }

