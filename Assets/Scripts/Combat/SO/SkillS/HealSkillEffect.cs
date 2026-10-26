using System.Collections.Generic;
using UnityEngine;

public class HealSkillEffect : SkillEffect
{
    [Range(0f, 1f)]
    public float baseHealValue;
    [Range(0f, 1f)]
    public float maxHealValue;
    private float K = 200f;


    public override void ExecuteEffect(CombatParticipantData user, List<CombatParticipantData> targets)
    {
        float MATK = user.stats.GetStat(CombatStat.MATK);
        float healPercent = baseHealValue + (maxHealValue - baseHealValue) * (MATK / (MATK + K));
        foreach(CombatParticipantData target in targets)
        {
            float healValue = target.currentHP * healPercent;
            target.ModHP(healValue);
        }
    }
}
