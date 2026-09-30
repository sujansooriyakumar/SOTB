using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItemSO", menuName = "SOTB/Inventory/WeaponItemSO")]
public class WeaponItemSO : SellableItemSO
{
    public WeaponClassSO weaponClass;
    public DamageAttribute damageAttribute;
    public int weaponStrength;
    public CombatStat[] damageStat;



    
}
