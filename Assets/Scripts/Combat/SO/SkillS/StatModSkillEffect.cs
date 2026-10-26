using System;
using System.Collections.Generic;
using UnityEngine;

public class StatModSkillEffect : SkillEffect
{
    public StatMod statMod;

    public override void ExecuteEffect(CombatParticipantData user_, List<CombatParticipantData> targets_)
    {
        foreach (CombatParticipantData target in targets_)
        {
            target.ActivateStatMod(statMod);
        }
    }
}

[Serializable]
public struct StatMod
{
    public CombatStat stat;
    public float mod;
    public int duration;
}