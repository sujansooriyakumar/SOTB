using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentData
{
    public WeaponItemSO equippedWeapon;
    public Dictionary<EquipmentSlot, EquipmentItemSO> equipment = new();

    public EquipmentData()
    {
        equipment.Add(EquipmentSlot.WEAPON, null);
        equipment.Add(EquipmentSlot.HEAD, null);
        equipment.Add(EquipmentSlot.CHEST, null);
        equipment.Add(EquipmentSlot.ARMS, null);
        equipment.Add(EquipmentSlot.ACCESSORY, null);

        equippedWeapon = null;
    }

    public EquipmentData CopyEquipement()
    {
        EquipmentData copy = new();
        foreach(KeyValuePair<EquipmentSlot, EquipmentItemSO> kvp in equipment)
        {
            copy.equipment[kvp.Key] = kvp.Value;
        }
        return copy;
    }

    public void EquipWeapon(int weaponToEquip)
    {
        equippedWeapon = InventoryController.Instance.GetItemData(weaponToEquip) as WeaponItemSO;
    }

    public void EquipArmor(int armorToEquip)
    {
        ArmorItemSO armor = InventoryController.Instance.GetItemData(armorToEquip) as ArmorItemSO;
        equipment[armor.equipmentSlot] = armor;
    }

    public void EquipItem(int itemToEquip)
    {
        EquipmentItemSO equipmentItem = InventoryController.Instance.GetItemData(itemToEquip) as EquipmentItemSO;
        equipment[equipmentItem.equipmentSlot] = equipmentItem;
    }

    public void InitializeEquipment(Dictionary<EquipmentSlot, EquipmentItemSO> equipment_)
    {
        equipment = equipment_;
    }

}




