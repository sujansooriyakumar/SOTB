using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private Queue<CombatParticipantData> turnOrder;

    public static BattleManager Instance;

    public BattleState battleState;
 
    public event Action<CombatParticipantData> OnTurnStart;
    public event Action<CombatParticipantData> OnTurnComplete;
    public event Action<BattleState> OnBattleStateUpdated;
    public event Action OnBattleComplete;
    private bool actionDone = false;
    private CombatAction combatAction;
    [Header("Debug")]
    public CombatEncounterSO sample;
    private void Awake()
    {
        InitializeSingleton();
    }

    private void InitializeSingleton()
    {
        if(Instance!= null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    [ContextMenu("Debug")]
    public void DebugEncounter()
    {
        InitializeBattle(sample);
    }
    public void InitializeBattle(CombatEncounterSO encounter_)
    {
        battleState = new BattleState(encounter_);
        UIController.Instance.Open<UICombatScreen>(p => p.SetBattleState(battleState));
      

       
        
    }
    [ContextMenu("End Turn")]
    public void DebugTurnEnd()
    {
        battleState.enemyTeam[0].ModHP(-400);
        battleState.EndTurn();
    }
    private IEnumerator BattleCoroutine()
    {
        // while enemy team list or palyer team list all have hp > 0
        while(battleState.IsPlayerTeamAlive() && battleState.IsEnemyTeamAlive())
        {
            battleState.StartTurn();
            OnTurnStart?.Invoke(battleState.GetTurnPlayer());
            yield return AwaitPlayerInput();
            OnTurnComplete?.Invoke(battleState.GetTurnPlayer());
            battleState.EndTurn();
            yield return null;
        }
        yield return null;
    }

    private IEnumerator AwaitPlayerInput()
    {
        actionDone = false;
        if (battleState.IsPlayer(battleState.GetTurnPlayer()))
        {
            yield return new WaitUntil(() => actionDone);
            combatAction.ExecuteAction();
        }
        else
        {
            // ai logic here
        }
        yield return null;
    }

    public void SubmitAction(CombatAction action)
    {
        combatAction = action;
        actionDone = true;
    }

 
}
