using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITargetSelectScreen:UIMenu
{
    private List<CombatParticipantData> targetList = new();
    [SerializeField] private Button buttonPrefab;
    [SerializeField] private Transform buttonParent;
    private List<Button> buttons = new();
    public event Action<CombatParticipantData> OnTargetChosen;
    private Dictionary<Button, CombatParticipantData> targetLookup = new();
    protected override void OnOpen()
    {
        foreach(CombatParticipantData participant in targetList)
        {
            Button button = Instantiate(buttonPrefab, buttonParent);
            button.GetComponentInChildren<TMP_Text>().text = participant.characterName;
            buttons.Add(button);
            targetLookup.Add(button, participant);
            button.onClick.AddListener(() => SelectTarget(button));

        }
    }
    protected override GameObject GetDefaultSelection() =>
    buttons.Count > 0 ? buttons[0].gameObject : null;

    private void SelectTarget(Button b)
    {
        OnTargetChosen?.Invoke(targetLookup[b]);
    }
    protected override void OnClose()
    {
        foreach(Button b in buttons)
        {
            b.onClick.RemoveAllListeners();
            Destroy(b.gameObject);
        }
        buttons.Clear();
        base.OnClose();
    }

    protected override void OnFocus()
    {
        base.OnFocus();
    }

    protected override void OnUnfocus()
    {
        base.OnUnfocus();
    }

    public void InitializeTargetList(List<CombatParticipantData> targetList_)
    {
       targetList = targetList_;
    }
    
}
