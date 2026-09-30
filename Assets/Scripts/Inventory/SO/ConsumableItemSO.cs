using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableItemSO", menuName = "SOTB/Inventory/ConsumableItemSO")]
public class ConsumableItemSO : SellableItemSO
{
    public List<EffectSO> effects;
    public bool usableInBattle;
    public bool usableOutOfBattle;

    public void UseItem(CombatParticipantData[] targets)
    {
        foreach(EffectSO effect in effects)
        {
            effect.ExecuteEffect(targets);
        }
    }
}
