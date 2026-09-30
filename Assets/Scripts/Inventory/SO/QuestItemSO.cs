using UnityEngine;

[CreateAssetMenu(fileName = "QuestItemSO", menuName = "SOTB/Inventory/QuestItemSO")]
public class QuestItemSO : ItemSO
{
    public QuestSO questReference;

    public override InventoryItemData CreateRuntimeData()
    {
        return new QuestItemData(this);
    }
}
