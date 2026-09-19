using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabaseSO", menuName = "SOTB/Inventory/ItemDatabaseSO")]
public class ItemDatabaseSO : ScriptableObject
{
    public List<ItemSO> items;

    public Dictionary<int, ItemSO> InitializeItemDictionary()
    {
        if (!ValidateDatabase())
        {
            Debug.LogError("Duplicate UID found");
            return null;
        }
        Dictionary<int, ItemSO> itemDictionary = new Dictionary<int, ItemSO>();

        foreach(ItemSO item in items)
        {
            itemDictionary.Add(item.uID, item);
        }

        return itemDictionary;
    }

    public bool ValidateDatabase()
    {
        HashSet<int> ids = new HashSet<int>();
        foreach (ItemSO item in items)
        {
            if (!ids.Add(item.uID))
            {
                return false;
            }
        }

        return true;
    }
}
