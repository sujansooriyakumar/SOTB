using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[Serializable]
public class CombatParticipantData
{
    public string characterName;
    public int characterID;
    public int maxHP;
    public int currentHP;
    public int maxSP;
    public int currentSP;
    public int level;

    [Header("Weaknesses")]
    public List<DamageType> typeWeaknesses;
    public List<DamageAttribute> attributeWeaknesses;

    [Header("Resistances")]
    public List<DamageType> typeResistances;
    public List<DamageAttribute> attributeResistances;

    public CombatStats stats;

    public EquipmentData equipmentData;

    public CombatParticipantData(CombatParticipantSO config)
    {
        characterName = config.character.characterName;
        characterID = config.character.characterID;
        maxHP = config.maxHP;
        maxSP = config.maxSP;
        level = config.level;

        stats = new CombatStats(config);

        currentHP = maxHP;
        currentSP = maxSP;

        equipmentData = new EquipmentData();
        equipmentData.InitializeArmor(config.armor);

        if (config.equippedWeapon != null)
        {
            equipmentData.EquipWeapon(config.equippedWeapon.uID);
        }

        attributeResistances = config.attributeResistances;
        typeResistances = config.typeResistances;

        attributeWeaknesses = config.attributeWeaknesses;
        typeWeaknesses = config.typeWeaknesses;
        CalculateSubstats();
    }

    public void CalculateSubstats()
    {
        stats.subStats[CombatStat.ATK] = CalculateAtk(equipmentData);
        stats.subStats[CombatStat.MATK] = CalculateMAtk(equipmentData);
        stats.subStats[CombatStat.PDEF] = CalculatePDef(equipmentData);
        stats.subStats[CombatStat.MDEF] = CalculateMDef(equipmentData);
        stats.subStats[CombatStat.DODGE] = CalculateDodge(equipmentData);
        stats.subStats[CombatStat.HIT] = CalculateHit(equipmentData);
        stats.subStats[CombatStat.CRIT] = CalculateCrit(equipmentData);
    }



    public int CalculateAtk(EquipmentData equipmentData_)
    {
        var weaponStats = equipmentData_.equippedWeapon?.damageStat;
        EquipmentData t = equipmentData_;
        float attackStat = 0;
        if (weaponStats == null || weaponStats.Length == 0)
            return stats.GetStat(CombatStat.STR);
        attackStat = (float)weaponStats.Average(stats.GetStat) * (1f + equipmentData_.equippedWeapon.weaponStrength / 100f);
        return Mathf.CeilToInt(attackStat);
    }

    public int CalculateMAtk(EquipmentData equipmentData_)
    {
        float mAttackStat = stats.GetStat(CombatStat.INT) * (1f + equipmentData_.equippedWeapon.weaponStrength / 100f);
        return Mathf.CeilToInt(mAttackStat);
    }


    public int CalculatePDef(EquipmentData equipmentData_)
    {
        float armorRating = 0f;
        foreach (ArmorItemSO armor in equipmentData_.equippedArmor.Values)
        {
            if (armor != null)
                armorRating += armor.PDEFRating;
        }

        return Mathf.CeilToInt(stats.GetStat(CombatStat.DEF) * (1f + armorRating * 0.01f));
    }

    public int CalculateMDef(EquipmentData equipmentData_)
    {
        float armorRating = 0f;
        foreach (ArmorItemSO armor in equipmentData_.equippedArmor.Values)
        {
            if (armor != null)
                armorRating += armor.MDEFRating;
        }

        return Mathf.CeilToInt(stats.GetStat(CombatStat.SPI) * (1f + armorRating * 0.01f));
    }

    public int CalculateDodge(EquipmentData equipmentData_)
    {
        float result = level + stats.GetStat(CombatStat.DEX) + (stats.GetStat(CombatStat.LUK) / 3f);
        return Mathf.CeilToInt(result);
    }

    public int CalculateHit(EquipmentData equipmentData_)
    {
        float result = level + stats.GetStat(CombatStat.AGI) + (stats.GetStat(CombatStat.LUK) / 5f);
        return Mathf.CeilToInt(result);
    }

    public int CalculateCrit(EquipmentData equipmentData_)
    {
        float result = level + stats.GetStat(CombatStat.LUK) / 3f;
        return Mathf.CeilToInt(result);
    }
}
