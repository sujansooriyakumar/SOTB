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
    public Dictionary<ArmorSlot, ArmorItemSO> armor = CreateDefault();

    public WeaponItemSO equippedWeapon;
 

    [Header("Weaknesses")]
    public List<DamageType> typeWeaknesses;
    public List<DamageAttribute> attributeWeaknesses;

    [Header("Resistances")]
    public List<DamageType> typeResistances;
    public List<DamageAttribute> attributeResistances;

    static Dictionary<ArmorSlot, ArmorItemSO> CreateDefault()
    {
        var dict = new Dictionary<ArmorSlot, ArmorItemSO>();
        foreach (ArmorSlot slot in Enum.GetValues(typeof(ArmorSlot)))
            dict[slot] = null;
        return dict;
    }

    void OnValidate()
    {
        armor ??= new Dictionary<ArmorSlot, ArmorItemSO>();
        foreach (ArmorSlot slot in Enum.GetValues(typeof(ArmorSlot)))
            if (!armor.ContainsKey(slot)) armor[slot] = null;
    }
}
