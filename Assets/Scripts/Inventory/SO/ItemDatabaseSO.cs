using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabaseSO", menuName = "SOTB/Inventory/ItemDatabaseSO")]
public class ItemDatabaseSO : ScriptableObject
{
    public List<ItemSO> items;

    public Dictionary<int, ItemSO> InitializeItemDictionary()
    {
        Dictionary<int, ItemSO> itemDictionary = new Dictionary<int, ItemSO>();

        foreach(ItemSO item in items)
        {
            itemDictionary.Add(item.uID, item);
        }

        return itemDictionary;
    }
}
