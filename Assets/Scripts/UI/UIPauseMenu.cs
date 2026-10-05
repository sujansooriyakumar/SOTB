using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPauseMenu : UIMenu
{
    [Header("Party Display")]
    [SerializeField] private Transform frontlineParent;
    [SerializeField] private Transform backlineParent;
    [SerializeField] private GameObject partyMemberDisplayPrefab;
    [SerializeField] private int frontlineSize = 4;

    [Header("Buttons")]
    [SerializeField] private Button inventoryButton;
    [SerializeField] private Button statusButton;

    private readonly List<GameObject> partyObjects = new();

    private void Awake()
    {
        inventoryButton.onClick.AddListener(() => UIController.Instance.Open<UIInventoryMenu>());
    }

    protected override void OnOpen()
    {
        List<CombatParticipantData> party = PartyManager.Instance.GetParty();
        for (int i = 0; i < party.Count; i++)
        {
            Transform parent = i < frontlineSize ? frontlineParent : backlineParent;
            InstantiatePartyDisplay(parent, party[i]);
        }
    }

    protected override void OnClose()
    {
        foreach (GameObject display in partyObjects)
        {
            Destroy(display);
        }
        partyObjects.Clear();
    }

    private void InstantiatePartyDisplay(Transform parent, CombatParticipantData partyMember)
    {
        GameObject display = Instantiate(partyMemberDisplayPrefab, parent);
        partyObjects.Add(display);

        if (display.TryGetComponent(out CharacterDisplayUI ui))
        {
            ui.InitializeDisplay(partyMember);
        }
    }
}