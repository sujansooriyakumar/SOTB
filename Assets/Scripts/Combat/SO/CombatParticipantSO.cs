using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CombatParticipantSO", menuName = "SOTB/Combat/CombatParticipantSO")]
public class CombatParticipantSO : SerializedScriptableObject
{
    public CharacterSO character;
    public int maxHP;
    public int maxSP;
    public int level;
   
    [Header("Stats")]
    public int STR;
    public int DEF;
    public int INT;
    public int SPI;
    public int DEX;
    public int AGI;
    public int LUK;

    [Header("Equipment")]
    [OdinSerialize]
    [DictionaryDrawerSettings(IsReadOnly = true, KeyLabel = "Slot", ValueLabel = "Armor",
                              DisplayMode = DictionaryDisplayOptions.OneLine)]
    public Dictionary<EquipmentSlot, EquipmentItemSO> equipment = CreateDefault();
 

    [Header("Weaknesses")]
    public List<DamageType> typeWeaknesses;
    public List<DamageAttribute> attributeWeaknesses;

    [Header("Resistances")]
    public List<DamageType> typeResistances;
    public List<DamageAttribute> attributeResistances;

    static Dictionary<EquipmentSlot, EquipmentItemSO> CreateDefault()
    {
        var dict = new Dictionary<EquipmentSlot, EquipmentItemSO>();
        foreach (EquipmentSlot slot in Enum.GetValues(typeof(EquipmentSlot)))
            dict[slot] = null;
        return dict;
    }

    void OnValidate()
    {
        equipment ??= new Dictionary<EquipmentSlot, EquipmentItemSO>();

        foreach (EquipmentSlot slot in Enum.GetValues(typeof(EquipmentSlot)))
        {
            if (!equipment.TryGetValue(slot, out var item))
            {
                equipment[slot] = null;
            }
            else if (item != null && item.equipmentSlot != slot)
            {
                Debug.LogWarning($"{item.name} goes in {item.equipmentSlot}, not {slot}. Cleared.", this);
                equipment[slot] = null;
            }
        }
    }
}
