using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class SampleDebug : SerializedMonoBehaviour
{
    public CombatParticipantSO attacker;

    public void Start()
    {
       AttackData sampleAttack = new AttackData();
        CombatStat[] stat = new CombatStat[1];
        stat[0] = CombatStat.STR;
        sampleAttack.primaryAttribute = stat;
        sampleAttack.damageType = DamageType.BLUNT;
        sampleAttack.damageAttribute = DamageAttribute.FIRE;
        sampleAttack.basePower = 110;

        for (int i = 0; i < 50; i++)
        {
            Debug.Log(DamageCalculator.CalculateDamage(new CombatParticipantData(attacker), new CombatParticipantData(attacker), sampleAttack));
        }
    }
}
