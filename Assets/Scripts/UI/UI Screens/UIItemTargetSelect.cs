using System.Collections.Generic;
using UnityEngine;

public class UIItemTargetSelect : UIMenu
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform buttonParent;

    private readonly List<ItemTargetSelectButton> targetButtons = new();
    private ConsumableItemSO itemToUse;

    public void SetItem(ConsumableItemSO item)
    {
        itemToUse = item;
    }

    protected override void OnOpen()
    {
        foreach (CombatParticipantData participant in PartyManager.Instance.GetParty())
        {
            ItemTargetSelectButton targetButton =
                Instantiate(buttonPrefab, buttonParent).GetComponent<ItemTargetSelectButton>();
            targetButton.InitializeButton(participant);
            targetButton.OnTargetSelected += UseItem;
            targetButtons.Add(targetButton);
        }
    }

    protected override void OnClose()
    {
        foreach (ItemTargetSelectButton button in targetButtons)
        {
            button.OnTargetSelected -= UseItem;
            Destroy(button.gameObject);
        }
        targetButtons.Clear();
        itemToUse = null;
    }

    protected override GameObject GetDefaultSelection()
    {
        return targetButtons.Count > 0 ? targetButtons[0].gameObject : null;
    }

    private void UseItem(CombatParticipantData target)
    {
        if (itemToUse == null) return;



        InventoryController.Instance.UseItem(itemToUse.uID, target);
        UIController.Instance.ReturnTo<UIInventoryMenu>();
    }
}