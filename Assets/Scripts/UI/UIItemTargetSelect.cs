using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItemTargetSelect : UIMenu
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] Transform buttonParent;
    private ConsumableItemSO itemToUse;
    public override void ActivateMenu()
    {
        List<CombatParticipantData> party = PartyManager.Instance.GetParty();
        foreach(CombatParticipantData participant in party)
        {
            GameObject tmp = Instantiate(buttonPrefab, buttonParent);
            ItemTargetSelectButton targetSelectButton = tmp.GetComponent<ItemTargetSelectButton>();
            targetSelectButton.InitializeButton(participant);
            targetSelectButton.OnTargetSelected += UseItem;
            buttons.Add(tmp.GetComponent<Button>());
            tmp.GetComponent<ItemTargetSelectButton>().OnTargetSelected += UseItem;
        }
        EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
    }
    public void SetItem(ConsumableItemSO itemToUse_)
    {
        this.itemToUse = itemToUse_;
    }

    private void UseItem(CombatParticipantData target)
    {
        itemToUse.UseItem(target);
        UIController.Instance.CloseMenu();
        InventoryController.Instance.RemoveItem(itemToUse.uID);
    }

    public override void DeactivateMenu()
    {
       
        
        base.DeactivateMenu();
    }

    public override void CloseMenu()
    {
        foreach (Button button in buttons)
        {
            button.onClick.RemoveAllListeners();
            Destroy(button.gameObject);
        }
        buttons.Clear();
        base.CloseMenu();
    }
}
