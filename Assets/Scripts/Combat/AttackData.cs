using System;
using UnityEngine;

[Serializable]
public class AttackData
{
    public int basePower;
    public CombatStat[] primaryAttribute;
    public DamageType damageType;
    public DamageAttribute damageAttribute;
}
