using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DamageSkillEffect : SkillEffect
{
    public int damage;
    public CombatStat[] primaryAttribute;
    public DamageAttribute damageAttribute;
    public DamageType damageType;
    public override void ExecuteEffect(CombatParticipantData attacker, List<CombatParticipantData> targets)
    {
        AttackData attackData = new AttackData(this);
        foreach (CombatParticipantData target in targets)
        {
            float dmg = DamageCalculator.CalculateDamage(attacker, target, attackData);
            target.ModHP(-dmg);
        }
    }
}
