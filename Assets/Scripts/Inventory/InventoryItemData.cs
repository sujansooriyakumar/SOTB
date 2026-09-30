using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItemData
{
    public int uid;
    public int qty;

    public InventoryItemData(KeyValuePair<int, int> kvp)
    {
        uid = kvp.Key;
        qty = kvp.Value;
    }
    

}

