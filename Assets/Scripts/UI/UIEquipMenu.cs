using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEquipMenu : UIMenu
{
    [Header("Button Spawning")]
    [SerializeField] private Transform partyButtonParent;
    public GameObject buttonPrefab;
    private readonly List<CharacterDisplayButton> partyButtons = new();
    protected override GameObject GetDefaultSelection() =>
    partyButtons.Count > 0 ? partyButtons[0].gameObject : null;

    [Header("Equipment Slots")]
    [SerializeField] private TMP_Text weaponSlot;
    [SerializeField] private TMP_Text headArmorSlot;
    [SerializeField] private TMP_Text chestArmorSlot;
    [SerializeField] private TMP_Text legsArmorSlot;
    [SerializeField] private TMP_Text armsArmorSlot;
    [SerializeField] private TMP_Text accessorySlot;

    [Header("Stats")]
    [SerializeField] private StatDisplay statDisplay;

    protected override void OnOpen()
    {
        List<CombatParticipantData> party = PartyManager.Instance.GetParty();
        foreach(CombatParticipantData participant in party)
        {
            GameObject tmp = Instantiate(buttonPrefab, partyButtonParent);
            if(tmp.TryGetComponent<CharacterDisplayButton>(out CharacterDisplayButton characterDisplay))
            {
                characterDisplay.InitializeButton(participant);
                characterDisplay.OnSelected += SelectPartyMember;
                partyButtons.Add(characterDisplay);
            }
            
            // init button here
        }

    }

    private const string EmptySlotText = "---";

    private void SelectPartyMember(CombatParticipantData participant)
    {
        EquipmentData equipmentData = participant.equipmentData;

        weaponSlot.text = equipmentData.equippedWeapon != null
            ? equipmentData.equippedWeapon.itemName
            : EmptySlotText;

        headArmorSlot.text = GetArmorName(equipmentData, ArmorSlot.HEAD);
        chestArmorSlot.text = GetArmorName(equipmentData, ArmorSlot.CHEST);
        legsArmorSlot.text = GetArmorName(equipmentData, ArmorSlot.LEGS);
        armsArmorSlot.text = GetArmorName(equipmentData, ArmorSlot.ARMS);
        accessorySlot.text = GetArmorName(equipmentData, ArmorSlot.ACCESSORY);

        statDisplay.UpdateDisplay(participant);
    }

    private string GetArmorName(EquipmentData equipmentData, ArmorSlot slot)
    {
        if (equipmentData.equippedArmor.TryGetValue(slot, out var armor) && armor != null)
            return armor.itemName;

        return EmptySlotText;
    }

    protected override void OnClose()
    {
        foreach (CharacterDisplayButton button in partyButtons)
        {
            button.OnSelected -= SelectPartyMember;

            // Deactivate first: Destroy is deferred to the end of the frame.
            button.gameObject.SetActive(false);
            Destroy(button.gameObject);
        }
        partyButtons.Clear();
    }

    protected override void OnFocus()
    {
        base.OnFocus();
    }

    protected override void OnUnfocus()
    {
        base.OnUnfocus();
    }
}
