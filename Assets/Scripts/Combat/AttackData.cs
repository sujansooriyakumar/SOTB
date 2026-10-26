using System;
using UnityEngine;

[Serializable]
public class AttackData
{
    public int basePower;
    public CombatStat[] primaryAttribute;
    public DamageType damageType;
    public DamageAttribute damageAttribute;

    public AttackData(WeaponItemSO weapon)
    {
        basePower = weapon.baseAttackStrength;
        primaryAttribute = weapon.damageStat;
        damageType = weapon.weaponClass.damageType;
        damageAttribute = weapon.damageAttribute;
    }

    public AttackData(DamageSkillEffect skillEffect)
    {
        basePower = skillEffect.damage;
        damageType = skillEffect.damageType;
        damageAttribute = skillEffect.damageAttribute;
        primaryAttribute = skillEffect.primaryAttribute;
        
    }
    public AttackData()
    {

    }
}
