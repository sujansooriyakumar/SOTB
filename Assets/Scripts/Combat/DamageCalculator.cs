using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public static class DamageCalculator
{
    public static float CalculateDamage(CombatParticipantData attacker_, CombatParticipantData defender_, AttackData attackData_)
    {
        float damage = 0;

        int levelMod = ((2 *attacker_.level)/5) + 2;
        float attackAttributeValue = GetAttackAttribute(attackData_, attacker_);
        float defendAttributeValue = GetDefendAttribute(attackData_, defender_);

        float attackingStat = GetFinalAttackStat(attackAttributeValue, attacker_);
        float defendingStat = GetFinalDefenceStat(defendAttributeValue, defender_);
        float resistances = GetResistances(attackData_, defender_);
        float weaknesses = GetWeaknesses(attackData_, defender_);
        float damageRoll = GetDamageRoll();
        damage = ((levelMod * attackData_.basePower * (attackingStat / defendingStat)) / 50f) + 2f;
        if(!CheckHit(GetHitRate(attacker_), GetDodgeRate(defender_)))
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

    private static float GetDamageRoll()
    {
        return Random.Range(0.85f, 1.0f);
    }

    private static float GetAttackAttribute(AttackData attackData_, CombatParticipantData attacker_)
    {
        CombatStat[] attackAttributes = attackData_.primaryAttribute;

        /*
         * Get average of attacking stats
         */
        float attackAttributeValue = 0f;
        foreach (CombatStat att in attackAttributes)
        {
            attackAttributeValue += attacker_.stats.GetStat(att);
        }
        attackAttributeValue = attackAttributeValue / attackAttributes.Length;

        return attackAttributeValue;


    }

    private static float GetDefendAttribute(AttackData attackData_, CombatParticipantData defender_)
    {
        /*
         * Get defending stat
         */
        float defendAttributeValue = 0f;

        if (attackData_.damageType == DamageType.MAGIC)
        {
            defendAttributeValue = defender_.stats.GetStat(CombatStat.SPI);
        }
        else
        {
            defendAttributeValue = defender_.stats.GetStat(CombatStat.DEF);
        }

        return defendAttributeValue;
    }

    private static float GetFinalAttackStat(float attackAttributeValue, CombatParticipantData attacker_)
    {
        float attackingStat = attackAttributeValue * (1f + attacker_.equipmentData.equippedWeapon.weaponStrength / 100f);
        return attackingStat;

    }

    private static float GetFinalDefenceStat(float defendAttributeValue, CombatParticipantData defender_)
    {
        float armorRating = 0f;
        foreach (KeyValuePair<ArmorSlot, ArmorData> kvp in defender_.equipmentData.equippedArmor)
        {
            if (kvp.Value != null)
            {
                armorRating += kvp.Value.armorRating;
            }
        }

        float defendingStat = defendAttributeValue * (1f + armorRating / 100f);
        return defendingStat;

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

    private static float GetHitRate(CombatParticipantData attacker_)
    {
        float result = 175f + attacker_.level + attacker_.stats.GetStat(CombatStat.DEX) + (attacker_.stats.GetStat(CombatStat.LUK) / 3f);
        return result;
    }

    private static float GetDodgeRate(CombatParticipantData defender_)
    {
        float result = 100f + defender_.level + defender_.stats.GetStat(CombatStat.AGI) + (defender_.stats.GetStat(CombatStat.LUK) / 5f);
        return result;
    }

    private static bool CheckHit(float hitRate, float dodgeRate)
    {
        bool result = false;
       // Debug.Log("Hit Rate: " + hitRate);
       // Debug.Log("Dodge Rate: " + dodgeRate);

        float hitChance = (hitRate - dodgeRate)/100f;
        float rng = Random.Range(0f, 1f);
        if(rng <= hitChance)
        {
            return true;
        }
        return result;

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
