using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class SampleDebug : SerializedMonoBehaviour
{
    public CombatParticipantSO attacker;
    public int basePower;
    public CombatStat[] primaryAttribute;
    public DamageAttribute damageAttribute;
    public DamageType damageType;


    [Button]
    public void CheckAttack()
    {
       AttackData sampleAttack = new AttackData();
     
        sampleAttack.primaryAttribute = primaryAttribute;
        sampleAttack.damageType = damageType;
        sampleAttack.damageAttribute = damageAttribute;
        sampleAttack.basePower = basePower;
        Debug.Log(DamageCalculator.CalculateDamage(new CombatParticipantData(attacker), new CombatParticipantData(attacker), sampleAttack));

     
    }
}
