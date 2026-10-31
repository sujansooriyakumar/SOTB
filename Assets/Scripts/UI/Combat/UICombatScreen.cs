using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Action Buttons")]
    [SerializeField] private Button attackButton;

    private CombatAction combatAction;
    private ActionType actionType;
    private List<CombatParticipantData> actionTargets;
    private SkillSO actionSkill;
    private ConsumableItemSO actionItem;

    private void Awake()
    {
        attackButton.onClick.AddListener(AttackPressed);
    }

    public void AttackPressed()
    {
        actionType = ActionType.ATTACK;
        UITargetSelectScreen targetSelect = UIController.Instance.Open<UITargetSelectScreen>(m => m.InitializeTargetList(battleState.enemyTeam));
        targetSelect.OnTargetChosen += SubmitTarget;
    }

    private void SubmitTarget(CombatParticipantData target_)
    {
        actionTargets = new List<CombatParticipantData>();
        actionTargets.Add(target_);
        SubmitAction();
    }

    private void SubmitAction()
    {
        CombatParticipantData user = battleState.GetTurnPlayer();
        switch (actionType)
        {
            case ActionType.ATTACK:
                combatAction = new AttackCombatAction(user, actionTargets[0]);
                break;
            case ActionType.SKILL:
                combatAction = new SkillCombatAction(user, actionTargets, actionSkill);
                break;
            case ActionType.ITEM:
                combatAction = new ItemCombatAction(actionItem, actionTargets);
                break;
            case ActionType.RUN:
                combatAction = new EscapeCombatAction(user, battleState.enemyTeam);
                break;
        }
        BattleManager.Instance.SubmitAction(combatAction);
        UIController.Instance.ReturnTo<UICombatScreen>();

    }

    private void SubmitSkill(CombatParticipantData target_)
    {
        
    }

    protected override void OnOpen()
    {

        characterDisplays.Clear();
        turnOrderDisplay.InitializeTurnOrder(battleState);
        List<CombatParticipantData> playerTeam = battleState.playerTeam;
        List<CombatParticipantData> enemyTeam = battleState.enemyTeam;

        foreach(CombatParticipantData participant in playerTeam)
        {
            CombatCharacterDisplay characterDisplay = Instantiate(participantDisplay, playerTeamParent);
            characterDisplay.InitUI(participant);
            characterDisplays.Add(participant, characterDisplay);
        }

        foreach (CombatParticipantData participant in enemyTeam)
        {
            CombatCharacterDisplay characterDisplay = Instantiate(participantDisplay, enemyTeamParent);
            characterDisplay.InitUI(participant);
            characterDisplays.Add(participant, characterDisplay);
        }
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

public enum ActionType
{
    ATTACK,
    SKILL,
    ITEM,
    RUN
}
