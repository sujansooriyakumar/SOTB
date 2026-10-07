using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public static class DamageCalculator
{

    private const float BaseHitChance = 0.80f;   // chance to hit when hit == dodge
    private const float PerPointChance = 0.01f;  // each point of difference = 1%
    private const float MinHitChance = 0.05f;
    private const float MaxHitChance = 1.0f;
    public static float CalculateDamage(CombatParticipantData attacker_, CombatParticipantData defender_, AttackData attackData_)
    {
        float damage = 0;

        int levelMod = ((2 *attacker_.level)/5) + 2;

        float attackingStat = attacker_.stats.GetStat(CombatStat.ATK);
        float defendingStat = attackData_.damageType == DamageType.MAGIC ? defender_.stats.GetStat(CombatStat.MDEF) : defender_.stats.GetStat(CombatStat.PDEF);
        float resistances = GetResistances(attackData_, defender_);
        float weaknesses = GetWeaknesses(attackData_, defender_);
        float damageRoll = GetDamageRoll();
        damage = ((levelMod * attackData_.basePower * (attackingStat / defendingStat)) / 50f) + 2f;
        if(!CheckHit(attacker_.stats.GetStat(CombatStat.HIT), defender_.stats.GetStat(CombatStat.DODGE)))
        {
            return 0f;
        }
        float critMultiplier = 1.0f;
        if (CheckCrit(attacker_))
        {
            Debug.Log("Critical Hit!");
            critMultiplier = 1.5f;
        }
        return damage * resistances * weaknesses * damageRoll * critMultiplier;
    }

    public static float CalculateItemDamage(CombatParticipantData defender_, float strength, DamageAttribute damageAttribute, DamageType damageType)
    {
        float damage = 0f;
        AttackData attackData = new AttackData();
        attackData.damageAttribute = damageAttribute;
        attackData.damageType = damageType;
        float effectiveness = GetResistances(attackData, defender_);
        damage = strength * effectiveness;
        return damage;
    }

    private static float GetDamageRoll()
    {
        return Random.Range(0.85f, 1.0f);
    }

   




    private static float GetWeaknesses(AttackData attackData_, CombatParticipantData defender_)
    {
        float result = 1.0f;
        foreach(DamageType damageType in defender_.typeWeaknesses)
        {
            if(damageType == attackData_.damageType)
            {
                result *= 1.5f;
            }
        }

        foreach(DamageAttribute damageAttribute in defender_.attributeWeaknesses)
        {
            if(damageAttribute == attackData_.damageAttribute)
            {
                result *= 1.5f;
            }
        }

        return result;
    }

    private static float GetResistances(AttackData attackData_, CombatParticipantData defender_)
    {
        float result = 1.0f;
        foreach (DamageType damageType in defender_.typeResistances)
        {
            if (damageType == attackData_.damageType)
            {
                result /= 1.5f;
            }
        }

        foreach (DamageAttribute damageAttribute in defender_.attributeResistances)
        {
            if (damageAttribute == attackData_.damageAttribute)
            {
                result /= 1.5f;
            }
        }

        return result;
    }



    private static bool CheckHit(float hitRate, float dodgeRate)
    {
        float hitChance = BaseHitChance + (hitRate - dodgeRate) * PerPointChance;
        hitChance = Mathf.Clamp(hitChance, MinHitChance, MaxHitChance);

        return Random.value < hitChance;
    }

    public static bool CheckCrit(CombatParticipantData attacker_)
    {
        bool result = false;
        float luck = attacker_.stats.GetStat(CombatStat.LUK);
        float K = 50f;
        float critRate = (50f * (luck / (luck + K)) * ((100f + K) / 100f))/100f;

        float rng = Random.Range(0f, 1f);
        if (rng <= critRate)
        {
            result = true;
        }
        return result;
    }

}
