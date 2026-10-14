using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Two modes in one screen:
//   Character mode: party list is interactable, slot panel is visible but dimmed.
//   Slot mode:      slot panel is interactable for the chosen character. Cancel goes back.
public class UIEquipMenu : UIMenu
{
    private const string EmptySlotText = "---";

    [Header("Panels")]
    [Tooltip("CanvasGroup on the party list panel. A child of this menu, not the menu root.")]
    [SerializeField] private CanvasGroup characterListGroup;
    [Tooltip("CanvasGroup on the equipment slot panel. A child of this menu, not the menu root.")]
    [SerializeField] private CanvasGroup slotPanelGroup;
    [Range(0f, 1f)]
    [SerializeField] private float inactivePanelAlpha = 0.5f;

    [Header("Button Spawning")]
    [SerializeField] private Transform partyButtonParent;
    [SerializeField] private GameObject buttonPrefab;

    [Header("Equipment Slots")]
    [Tooltip("In order: weapon, head, chest, legs, arms, accessory.")]
    [SerializeField] private Button[] slotButtons;
    [SerializeField] private TMP_Text weaponSlot;
    [SerializeField] private TMP_Text headArmorSlot;
    [SerializeField] private TMP_Text chestArmorSlot;
    [SerializeField] private TMP_Text legsArmorSlot;
    [SerializeField] private TMP_Text armsArmorSlot;
    [SerializeField] private TMP_Text accessorySlot;

    [Header("Stats")]
    [SerializeField] private StatDisplay statDisplay;

    private readonly List<CharacterDisplayButton> partyButtons = new();
    private CombatParticipantData selectedParticipant;
    private GameObject selectedCharacterButton;
    private bool choosingSlot;

    private void Awake()
    {
        for (int i = 0; i < slotButtons.Length; i++)
        {
            int slotIndex = i;
            slotButtons[i].onClick.AddListener(() => OnSlotChosen(slotIndex));
        }

        SetVerticalNavigation(slotButtons);
    }

    protected override void OnOpen()
    {
        List<Selectable> characterSelectables = new();

        foreach (CombatParticipantData participant in PartyManager.Instance.GetParty())
        {
            GameObject buttonObject = Instantiate(buttonPrefab, partyButtonParent);
            if (!buttonObject.TryGetComponent(out CharacterDisplayButton characterButton)) continue;

            characterButton.InitializeButton(participant);
            characterButton.OnSelected += PreviewPartyMember;

            // Submit/click on a character switches to slot mode for that character.
            if (buttonObject.TryGetComponent(out Button button))
            {
                button.onClick.AddListener(() => EnterSlotMode(participant, buttonObject));
                characterSelectables.Add(button);
            }

            partyButtons.Add(characterButton);
        }

        SetVerticalNavigation(characterSelectables.ToArray());
        SetMode(slotMode: false);

    }

    protected override void OnClose()
    {
        foreach (CharacterDisplayButton button in partyButtons)
        {
            button.OnSelected -= PreviewPartyMember;
            button.gameObject.SetActive(false);
            Destroy(button.gameObject);
        }
        partyButtons.Clear();

        selectedParticipant = null;
        selectedCharacterButton = null;
        choosingSlot = false;
    }

    protected override GameObject GetDefaultSelection()
    {
        if (choosingSlot) return slotButtons.Length > 0 ? slotButtons[0].gameObject : null;
        return partyButtons.Count > 0 ? partyButtons[0].gameObject : null;
    }

    // Cancel in slot mode goes back to the character list instead of closing the menu.
    public override bool HandleCancel()
    {
        if (!choosingSlot) return false;

        SetMode(slotMode: false);
        EventSystem.current.SetSelectedGameObject(selectedCharacterButton);
        return true;
    }

    private void EnterSlotMode(CombatParticipantData participant, GameObject characterButton)
    {
        selectedParticipant = participant;
        selectedCharacterButton = characterButton;

        SetMode(slotMode: true);
        EventSystem.current.SetSelectedGameObject(slotButtons[0].gameObject);
    }

    private void SetMode(bool slotMode)
    {
        choosingSlot = slotMode;

        characterListGroup.interactable = !slotMode;
        slotPanelGroup.interactable = slotMode;
        slotPanelGroup.alpha = slotMode ? 1f : inactivePanelAlpha;
    }

    private void OnSlotChosen(int slotIndex)
    {
        // Next step: open an item list for this slot, e.g.
        // UIController.Instance.Open<UIEquipItemList>(m => m.Setup(selectedParticipant, slotIndex));
        Debug.Log($"Chose slot {slotIndex} for {selectedParticipant}");
        UIController.Instance.Open<UIEquipmentDisplay>(m => m.SetEquipmentType((EquipmentSlot)slotIndex));

       
    }

    private void PreviewPartyMember(CombatParticipantData participant)
    {
        EquipmentData equipmentData = participant.equipmentData;

        weaponSlot.text = GetEquipmentName(equipmentData, EquipmentSlot.WEAPON);
        headArmorSlot.text = GetEquipmentName(equipmentData, EquipmentSlot.HEAD);
        chestArmorSlot.text = GetEquipmentName(equipmentData, EquipmentSlot.CHEST);
        legsArmorSlot.text = GetEquipmentName(equipmentData, EquipmentSlot.LEGS);
        armsArmorSlot.text = GetEquipmentName(equipmentData, EquipmentSlot.ARMS);
        accessorySlot.text = GetEquipmentName(equipmentData, EquipmentSlot.ACCESSORY);

        statDisplay.UpdateDisplay(participant);
    }

    private string GetEquipmentName(EquipmentData equipmentData, EquipmentSlot slot)
    {
        if (equipmentData?.equipment != null
            && equipmentData.equipment.TryGetValue(slot, out var item)
            && item != null)
        {
            return item.itemName;
        }

        return EmptySlotText;
    }

    // Up/down between neighbours with wrap-around, so the cursor can't jump to the other panel.
    private static void SetVerticalNavigation(Selectable[] selectables)
    {
        for (int i = 0; i < selectables.Length; i++)
        {
            selectables[i].navigation = new Navigation
            {
                mode = Navigation.Mode.Explicit,
                selectOnUp = selectables[(i - 1 + selectables.Length) % selectables.Length],
                selectOnDown = selectables[(i + 1) % selectables.Length]
            };
        }
    }
}