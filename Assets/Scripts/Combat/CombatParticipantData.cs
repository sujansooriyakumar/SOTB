using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CombatParticipantData
{
    public string characterName;
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
        characterName = config.characterName;
        maxHP = config.maxHP;
        maxSP = config.maxSP;
        level = config.level;

        stats = new CombatStats(config);

        currentHP = maxHP;
        currentSP = maxSP;

        equipmentData = new EquipmentData();
        foreach (KeyValuePair<ArmorSlot, ArmorItemSO> kvp in config.equippedArmor)
        {
            if (kvp.Value != null)
            {
                equipmentData.EquipArmor(kvp.Value);
            }
        }
        if (config.equippedWeapon != null)
        {
            equipmentData.EquipWeapon(config.equippedWeapon);
        }

        attributeResistances = config.attributeResistances;
        typeResistances = config.typeResistances;

        attributeWeaknesses = config.attributeWeaknesses;
        typeWeaknesses = config.typeWeaknesses;
    }
}
