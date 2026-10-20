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
        equipmentData.InitializeEquipment(config.equipment);

      /*  if (config.equippedWeapon != null)
        {
            equipmentData.EquipWeapon(config.equippedWeapon.uID);
        }
*/
        attributeResistances = config.attributeResistances;
        typeResistances = config.typeResistances;

        attributeWeaknesses = config.attributeWeaknesses;
        typeWeaknesses = config.typeWeaknesses;
        CalculateSubstats(stats, equipmentData);
    }

    public void CalculateSubstats(CombatStats stats_, EquipmentData equipmentData_)
    {
        stats_.stats[CombatStat.ATK] = CalculateAtk(equipmentData_);
        stats_.stats[CombatStat.MATK] = CalculateMAtk(equipmentData_);
        stats_.stats[CombatStat.PDEF] = CalculatePDef(equipmentData_);
        stats_.stats[CombatStat.MDEF] = CalculateMDef(equipmentData_);
        stats_.stats[CombatStat.DODGE] = CalculateDodge(equipmentData_);
        stats_.stats[CombatStat.HIT] = CalculateHit(equipmentData_);
        stats_.stats[CombatStat.CRIT] = CalculateCrit(equipmentData_);
    }

    public CombatStats PreviewStats(int newEquip, EquipmentSlot slot)
    {
        CombatStats newStats = stats.CopyStats();
        EquipmentData newEquipData = equipmentData.CopyEquipement();
        if (newEquip >= 0)
        {
            newEquipData.EquipItem(newEquip);
        }
        else
        {
            newEquipData.RemoveItem(slot);
        }
            CalculateSubstats(newStats, newEquipData);
        return newStats;
    }



    private static WeaponItemSO GetWeapon(EquipmentData equipmentData_)
    {
        if (equipmentData_?.equipment == null) return null;
      
        return equipmentData_.equipment.TryGetValue(EquipmentSlot.WEAPON, out var item) && item >= 0
            ? InventoryController.Instance.GetItemData(item) as WeaponItemSO
            : null;
    }

    public int CalculateAtk(EquipmentData equipmentData_)
    {
        WeaponItemSO weapon = GetWeapon(equipmentData_);
        if (weapon == null || weapon.damageStat == null || weapon.damageStat.Length == 0)
            return stats.GetStat(CombatStat.STR);

        float attackStat = (float)weapon.damageStat.Average(stats.GetStat) * (1f + weapon.weaponStrength / 100f);
        return Mathf.CeilToInt(attackStat);
    }

    public int CalculateMAtk(EquipmentData equipmentData_)
    {
        WeaponItemSO weapon = GetWeapon(equipmentData_);
        float weaponBonus = weapon != null ? weapon.weaponStrength / 100f : 0f;

        return Mathf.CeilToInt(stats.GetStat(CombatStat.INT) * (1f + weaponBonus));
    }


    public int CalculatePDef(EquipmentData equipmentData_)
    {
        float armorRating = 0f;
        foreach (var item in equipmentData_.equipment.Values)
        {
            if (item >= 0 && InventoryController.Instance.GetItemData(item) is ArmorItemSO armor)
                armorRating += armor.PDEFRating;
        }

        return Mathf.CeilToInt(stats.GetStat(CombatStat.DEF) * (1f + armorRating * 0.01f));
    }

    public int CalculateMDef(EquipmentData equipmentData_)
    {
        float armorRating = 0f;
        foreach (var item in equipmentData_.equipment.Values)
        {
            if (item >= 0 && InventoryController.Instance.GetItemData(item) is ArmorItemSO armor)
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
