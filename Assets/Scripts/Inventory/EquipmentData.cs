using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentData
{
    public WeaponItemSO equippedWeapon;
    public Dictionary<EquipmentSlot, int> equipment = new();

    public EquipmentData()
    {
        equipment.Add(EquipmentSlot.WEAPON, -1);
        equipment.Add(EquipmentSlot.HEAD, -1);
        equipment.Add(EquipmentSlot.CHEST, -1);
        equipment.Add(EquipmentSlot.ARMS, -1);
        equipment.Add(EquipmentSlot.LEGS, -1);
        equipment.Add(EquipmentSlot.ACCESSORY, -1);

        equippedWeapon = null;
    }

    public EquipmentData CopyEquipement()
    {
        EquipmentData copy = new();
        foreach(KeyValuePair<EquipmentSlot, int> kvp in equipment)
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
       // equipment[armor.equipmentSlot] = armorToEquip;
    }

    public void EquipItem(int itemToEquip)
    {
        EquipmentItemSO equipmentItem = InventoryController.Instance.GetItemData(itemToEquip) as EquipmentItemSO;
        equipment[equipmentItem.equipmentSlot] = itemToEquip;
    }

    public void RemoveItem(EquipmentSlot slot)
    {
        equipment[slot] = -1;
    }
    public void InitializeEquipment(Dictionary<EquipmentSlot, EquipmentItemSO> equipment_)
    {
        foreach (KeyValuePair<EquipmentSlot, EquipmentItemSO> kvp in equipment_)
        {
            if (kvp.Value != null)
            {
                equipment[kvp.Key] = kvp.Value.uID;
            }
        }
    }

}




