using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemCombatAction : CombatAction
{
    private ConsumableItemSO itemToUse;
    private List<CombatParticipantData> targets;
    public override event Action OnActionComplete;
    public ItemCombatAction(ConsumableItemSO itemToUse_, List<CombatParticipantData> targets_)
    {
        itemToUse = itemToUse_;
        targets = targets_;
    }
    public override void ExecuteAction()
    {
        List<ItemEffectSO> effects = itemToUse.effects;
        foreach (CombatParticipantData target in targets)
        {
            foreach (ItemEffectSO effect in effects)
            {
                effect.ExecuteEffect(target);
            }
        }
        OnActionComplete?.Invoke();
    }
}
