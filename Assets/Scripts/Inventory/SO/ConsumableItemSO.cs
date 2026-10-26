using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(fileName = "ConsumableItemSO", menuName = "SOTB/Inventory/ConsumableItemSO")]
public class ConsumableItemSO : SellableItemSO
{
    public List<ItemEffectSO> effects;
    public bool usableInBattle;
    public bool usableOutOfBattle;

    public bool UseItem(CombatParticipantData target)
    {
        bool usable = false;
        foreach(ItemEffectSO effect in effects)
        {
           usable |= effect.ExecuteEffect(target);
           
        }
        return usable;
    }

    public bool UseItem(List<CombatParticipantData> targets)
    {
        bool usable = false;

        foreach (CombatParticipantData target in targets)
        {
            foreach (ItemEffectSO effect in effects)
            {
                usable |= effect.ExecuteEffect(target);
            }
        }
        return usable;
    }
}
