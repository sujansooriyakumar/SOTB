using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPauseMenu : UIMenu
{
    [SerializeField] private Transform frontlineParent;
    [SerializeField] private Transform backlineParent;
    [SerializeField] private GameObject partyMemberDisplayPrefab;
    [SerializeField] private GameObject firstItem;
    private List<GameObject> partyObjects = new();

    [Header("Buttons")]
    [SerializeField] private Button statusButton;
    public override void ActivateMenu()
    {
        base.ActivateMenu();
        List<CombatParticipantData> party = PartyManager.Instance.GetParty();
        for (int i = 0; i < party.Count; i++)
        {
            if (i <= 3)
            {
               InstantiatePartyDisplay(frontlineParent, party[i]);
            }
            else
            {
              InstantiatePartyDisplay(backlineParent, party[i]);
            }
        }
        EventSystem.current.SetSelectedGameObject(firstItem);
        SetUpButtons();
    }

    private void SetUpButtons()
    {
        buttons[2].onClick.AddListener(() => UIController.Instance.OpenMenu(1));
    }

    private void InstantiatePartyDisplay(Transform parent, CombatParticipantData partyMember)
    {
        GameObject tmp = Instantiate(partyMemberDisplayPrefab, parent);
        partyObjects.Add(tmp);
        if (tmp.TryGetComponent<CharacterDisplayUI>(out CharacterDisplayUI ui))
        {
            ui.InitializeDisplay(partyMember);
        }
    }
    public override void DeactivateMenu()
    {
        base .DeactivateMenu();
        foreach (GameObject tmp in partyObjects)
        {
            Destroy(tmp);
        }
        partyObjects.Clear();
    }
}
