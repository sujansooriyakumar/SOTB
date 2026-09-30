using System;
using UnityEngine;

[Serializable]
public class WeaponItemData : SellableItemData
{
    public WeaponClassSO weaponClass;
    public DamageAttribute damageAttribute;
    public int weaponStrength;
    public CombatStat[] damageStat;
    public WeaponItemData(WeaponItemSO config_) : base(config_)
    {
        weaponClass = config_.weaponClass;
        damageAttribute = config_.damageAttribute;
        damageStat = config_.damageStat;
        weaponStrength = config_.weaponStrength;
        qty = 1;
        uid = config_.uID;
        itemName = config_.itemName;
        itemDescription = config_.itemDescription;
        sellPrice = config_.sellPrice;
    }
}
