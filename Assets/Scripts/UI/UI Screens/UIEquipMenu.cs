using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


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
    [Tooltip("In order: weapon, head, chest, legs, arms, accessoy.")]
    public List<EquipmentSlotDisplay> slotDisplays = new();
    Button[] slotButtons;

    [Header("Stats")]
    [SerializeField] private StatDisplay statDisplay;

    private readonly List<CharacterDisplayButton> partyButtons = new();
    private CombatParticipantData selectedParticipant;
    private GameObject selectedCharacterButton;
    private EquipmentSlot selectedSlot;
    private bool choosingSlot;

    private void Awake()
    {
        slotButtons = new Button[slotDisplays.Count];

        int i = 0;
        foreach(EquipmentSlotDisplay slotDisplay in slotDisplays)
        {
            slotDisplay.button.onClick.AddListener(() => OnSlotChosen(slotDisplay.slot));
            slotButtons[i] = slotDisplay.button;
            i++;
        }
      

        SetVerticalNavigation(slotButtons);
    }

    protected override void OnOpen()
    {
        List<Selectable> characterSelectables = new();

        foreach (CombatParticipantData participant in PartyManager.Instance.GetParty())
        {
            GameObject buttonObject = Instantiate(buttonPrefab, partyButtonParent);
            participant.OnCharacterUpdated += PreviewPartyMember;
            if (!buttonObject.TryGetComponent(out CharacterDisplayButton characterButton)) continue;

            characterButton.InitializeButton(participant);
            characterButton.OnSelected += PreviewPartyMember;

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
    protected override void OnFocus()
    {
        if(selectedParticipant != null) statDisplay.UpdateDisplay(selectedParticipant.stats);
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

    private void OnSlotChosen(EquipmentSlot slotIndex)
    {

        Debug.Log($"Chose slot {slotIndex} for {selectedParticipant}");
        UIEquipmentDisplay equipmentList = UIController.Instance.Open<UIEquipmentDisplay>(m => m.SetEquipmentType(slotIndex, selectedParticipant.equipmentData.equipment[slotIndex]));
        equipmentList.onItemSelected += PreviewNewEquipment;
        equipmentList.onItemChosen += HandleEquipment;
        selectedSlot = slotIndex;
    }
    private void HandleEquipment(int uid) {
        if(uid >= 0) selectedParticipant.equipmentData.EquipItem(uid);
        else
        {
            selectedParticipant.equipmentData.RemoveItem(selectedSlot);
        }
        UIController.Instance.ReturnTo<UIEquipMenu>();
        
    }
    private void PreviewNewEquipment(int newEquipment, EquipmentSlot slot)
    {
        CombatStats newStats = selectedParticipant.PreviewStats(newEquipment, slot);
        statDisplay.PreviewStats(newStats);
    }

    private void PreviewPartyMember(CombatParticipantData participant)
    {
        EquipmentData equipmentData = participant.equipmentData;

       for(int i = 0; i < slotDisplays.Count; i++)
        {
            EquipmentSlotDisplay display = slotDisplays[i];
            EquipmentItemSO item = GetEquippedItem(equipmentData, display.slot);
            display.equippedItem = item != null ? item.uID : -1;
            slotDisplays[i] = display;
            display.button.GetComponentInChildren<TMP_Text>().text =
                item != null ? item.itemName : "---";
            
        }

        statDisplay.UpdateDisplay(participant.stats);
    }
    private EquipmentItemSO GetEquippedItem(EquipmentData equipmentData, EquipmentSlot slot)
    {
        if (equipmentData?.equipment != null
            && equipmentData.equipment.TryGetValue(slot, out var item))
        {
            return item >= 0? InventoryController.Instance.GetItemData(item) as EquipmentItemSO : null;
        }

        return null;
    }
    private string GetEquipmentName(EquipmentData equipmentData, EquipmentSlot slot)
    {
        if (equipmentData?.equipment != null
            && equipmentData.equipment.TryGetValue(slot, out var item)
            && item >= 0)
        {
            return InventoryController.Instance.GetItemData(item).itemName;
        }

        return EmptySlotText;
    }

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

[Serializable]
public struct EquipmentSlotDisplay
{
    public EquipmentSlot slot;
    public int equippedItem;
    public Button button;
}