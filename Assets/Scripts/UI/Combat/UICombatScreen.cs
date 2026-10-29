using System.Collections.Generic;
using UnityEngine;

public class UICombatScreen : UIMenu
{
    private BattleState battleState;
    [SerializeField] private Transform playerTeamParent;
    [SerializeField] private Transform enemyTeamParent;
    [SerializeField] private Transform turnOrderParent;

    private Dictionary<CombatParticipantData, CombatCharacterDisplay> characterDisplays = new();

    [SerializeField] private CombatCharacterDisplay participantDisplay;
    [SerializeField] private GameObject turnIcon;
    [SerializeField] private TurnOrderDisplay turnOrderDisplay;
    protected override void OnOpen()
    {

        characterDisplays.Clear();
       /* foreach(CombatParticipantData p in battleState.turnOrder)
        {
            Transform parent = battleState.IsPlayer(p) ? playerTeamParent : enemyTeamParent;
            CombatCharacterDisplay characterDisplay = Instantiate(participantDisplay, parent);
            characterDisplay.InitUI(p);
            characterDisplays.TryAdd(p, characterDisplay);
            p.OnCharacterUpdated += RefreshUI;
            Instantiate(turnIcon, turnOrderParent);
        }*/
        turnOrderDisplay.InitializeTurnOrder(battleState);
    }

    protected override void OnClose()
    {
        foreach(KeyValuePair<CombatParticipantData, CombatCharacterDisplay> kvp in characterDisplays)
        {
            kvp.Key.OnCharacterUpdated -= RefreshUI;
            Destroy(kvp.Value.gameObject);
        }
        characterDisplays.Clear();
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

    public void SetBattleState(BattleState battleState_)
    {
        battleState = battleState_;
    }

    private void RefreshUI(CombatParticipantData p)
    {
        characterDisplays[p].InitUpdate(p);
    }
}
