using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CombatParticipantSO", menuName = "SOTB/Combat/CombatParticipantSO")]
public class CombatParticipantSO : SerializedScriptableObject
{
    public string characterName;
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
    public WeaponItemSO equippedWeapon;
    public Dictionary<ArmorSlot, ArmorItemSO> equippedArmor;

    [Header("Weaknesses")]
    public List<DamageType> typeWeaknesses;
    public List<DamageAttribute> attributeWeaknesses;

    [Header("Resistances")]
    public List<DamageType> typeResistances;
    public List<DamageAttribute> attributeResistances;
}
