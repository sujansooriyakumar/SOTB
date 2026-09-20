using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentData
{
    public WeaponData equippedWeapon;
    public Dictionary<ArmorSlot, ArmorData> equippedArmor = new();

    public EquipmentData()
    {
        equippedArmor.Add(ArmorSlot.HEAD, null);
        equippedArmor.Add(ArmorSlot.CHEST, null);
        equippedArmor.Add(ArmorSlot.ARMS, null);
        equippedArmor.Add(ArmorSlot.LEGS, null);
        equippedArmor.Add(ArmorSlot.ACCESSORY, null);

        equippedWeapon = null;
    }

    public void EquipWeapon(WeaponItemSO weaponToEquip)
    {
        equippedWeapon = new WeaponData(weaponToEquip);
    }

    public void EquipArmor(ArmorItemSO armorToEquip)
    {
        equippedArmor[armorToEquip.armorSlot] = new ArmorData(armorToEquip);
    }
}

public class WeaponData
{
    public string itemName;
    public int weaponStrength;
    public DamageAttribute damageAttribute;
    public DamageType damageType;

    public WeaponData(WeaponItemSO config)
    {
        itemName = config.itemName;
        weaponStrength = config.weaponStrength;
        damageType = config.weaponClass.damageType;
        damageAttribute = config.damageAttribute;
    }
}

public class ArmorData
{
    public string itemName;
    public int armorRating;
    public ArmorSlot armorSlot;
    public ArmorData(ArmorItemSO config)
    {
        itemName = config.itemName;
        armorRating = config.armorRating;
        armorSlot = config.armorSlot;
    }
}

