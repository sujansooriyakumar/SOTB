using System;
using UnityEngine;

[Serializable]

public class QuestItemData : InventoryItemData
{
    public QuestSO questReference;
    public QuestItemData(QuestItemSO config_) : base(config_)
    {
        questReference = config_.questReference;
        uid = config_.uID;
        itemDescription = config_.itemDescription;
        itemName = config_.itemName;
        qty = 1;
    }
}
