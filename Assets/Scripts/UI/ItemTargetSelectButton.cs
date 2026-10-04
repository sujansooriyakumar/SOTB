using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemTargetSelectButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private TMP_Text nameDisplay;
    [SerializeField] private TMP_Text hpDisplay;
    [SerializeField] private TMP_Text spDisplay;
    [SerializeField] private Image selected;
    private CombatParticipantData target;
    public event Action<CombatParticipantData> OnTargetSelected;
    public void InitializeButton(CombatParticipantData partyMember)
    {
        target = partyMember;
        nameDisplay.text = partyMember.characterName;
        hpDisplay.text = "HP: " + partyMember.currentHP + "/" + partyMember.maxHP;
        spDisplay.text = "SP: " + partyMember.currentSP + "/" + partyMember.maxSP;
        GetComponent<Button>().onClick.AddListener(SelectTarget);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selected.enabled = false;
    }

    public void OnSelect(BaseEventData eventData)
    {
        selected.enabled = true;
    }

    public void SelectTarget()
    {
        OnTargetSelected?.Invoke(target);
    }
}
