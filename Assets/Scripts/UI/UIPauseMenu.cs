using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPauseMenu : UIMenu
{
    [SerializeField] private Transform frontlineParent;
    [SerializeField] private Transform backlineParent;
    [SerializeField] private GameObject partyMemberDisplayPrefab;
    [SerializeField] private GameObject firstItem;
    public override void ActivateMenu()
    {
        List<CombatParticipantData> party = PartyManager.Instance.GetParty();
        for (int i = 0; i < party.Count; i++)
        {
            if (i <= 3)
            {
                GameObject tmp = Instantiate(partyMemberDisplayPrefab, frontlineParent);
                if (tmp.TryGetComponent<CharacterDisplayUI>(out CharacterDisplayUI ui))
                {
                    ui.InitializeDisplay(party[i]);
                }
            }
            else
            {
                GameObject tmp = Instantiate(partyMemberDisplayPrefab, backlineParent);
                if (tmp.TryGetComponent<CharacterDisplayUI>(out CharacterDisplayUI ui))
                {
                    ui.InitializeDisplay(party[i]);
                }
            }
        }
        EventSystem.current.SetSelectedGameObject(firstItem);
    }
    public override void DeactivateMenu()
    {
    }
}
