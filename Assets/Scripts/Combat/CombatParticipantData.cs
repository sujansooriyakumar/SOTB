using System;
using System.Collections.Generic;
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
        if (config.equippedArmor_accessory != null) equipmentData.EquipArmor(config.equippedArmor_accessory.uID);
        if (config.equippedArmor_chest != null) equipmentData.EquipArmor(config.equippedArmor_chest.uID);
        if (config.equippedArmor_hands != null) equipmentData.EquipArmor(config.equippedArmor_hands.uID);
        if (config.equippedArmor_helm != null) equipmentData.EquipArmor(config.equippedArmor_helm.uID);
        if (config.equippedArmor_legs != null) equipmentData.EquipArmor(config.equippedArmor_legs.uID);

        if (config.equippedWeapon != null)
        {
            equipmentData.EquipWeapon(config.equippedWeapon.uID);
        }

        attributeResistances = config.attributeResistances;
        typeResistances = config.typeResistances;

        attributeWeaknesses = config.attributeWeaknesses;
        typeWeaknesses = config.typeWeaknesses;
    }
}
