using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentData
{
    public WeaponItemSO equippedWeapon;
    public Dictionary<ArmorSlot, ArmorItemSO> equippedArmor = new();

    public EquipmentData()
    {
        equippedArmor.Add(ArmorSlot.HEAD, null);
        equippedArmor.Add(ArmorSlot.CHEST, null);
        equippedArmor.Add(ArmorSlot.ARMS, null);
        equippedArmor.Add(ArmorSlot.LEGS, null);
        equippedArmor.Add(ArmorSlot.ACCESSORY, null);

        equippedWeapon = null;
    }

    public void EquipWeapon(int weaponToEquip)
    {
        equippedWeapon = InventoryController.Instance.GetItemData(weaponToEquip) as WeaponItemSO;
    }

    public void EquipArmor(int armorToEquip)
    {
        ArmorItemSO armor = InventoryController.Instance.GetItemData(armorToEquip) as ArmorItemSO;
        equippedArmor[armor.armorSlot] = armor;
    }

    public void InitializeArmor(Dictionary<ArmorSlot, ArmorItemSO> armorToEquip)
    {
        equippedArmor = armorToEquip;
    }
}




