using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillSO", menuName = "SOTB/Combat/SkillSO")]
public class SkillSO : ScriptableObject
{
    [ShowInInspector]
    public List<SkillEffect> primaryEffects;
    [ShowInInspector]
    public List<ChanceEffect> chanceEffects;

    public void ExecuteSkill(CombatParticipantData user, List<CombatParticipantData> targets)
    {
        foreach (SkillEffect effect in primaryEffects)
        {
            effect.ExecuteEffect(user, targets);
        }

        foreach (ChanceEffect effect in chanceEffects)
        {
            float rng = UnityEngine.Random.value;
            if(rng < effect.chance)
            {
                effect.effect.ExecuteEffect(user, targets);
            }
        }
    }
}

[Serializable]
public struct ChanceEffect
{
    public float chance;
    [ShowInInspector] public SkillEffect effect;
}
