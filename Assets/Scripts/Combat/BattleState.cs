using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using System.Linq;
using System.Runtime.CompilerServices;
[Serializable]
public class BattleState
{
    public List<CombatParticipantData> playerTeam;
    public List<CombatParticipantData> enemyTeam;
    public Queue<CombatParticipantData> turnOrder;
    private CombatParticipantData turnPlayer;
    public event Action<BattleState> OnBattleStateChanged;

    public BattleState(CombatEncounterSO encounter)
    {
        playerTeam = PartyManager.Instance.GetParty();
        enemyTeam = new List<CombatParticipantData>();
        foreach(CombatParticipantSO enemy in encounter.enemies)
        {
            enemyTeam.Add(new CombatParticipantData(enemy));
        }
        InitTurnOder();

    }

    
    private void InitTurnOder()
    {
        List<CombatParticipantData> battleParticipants = playerTeam.Concat(enemyTeam).ToList();
        battleParticipants.OrderBy(tmp => tmp.stats.stats[CombatStat.AGI]);
        turnOrder = new Queue<CombatParticipantData>();
        foreach (CombatParticipantData p in battleParticipants)
        {
            turnOrder.Enqueue(p);
            p.OnCharacterDead += RemoveFromTurnOrder;
        }
    }

    public CombatParticipantData StartTurn()
    {
        turnPlayer = turnOrder.Peek();
        return turnPlayer;
    }

    private void RemoveFromTurnOrder(CombatParticipantData battleParticipant)
    {
        List<CombatParticipantData> tmp = turnOrder.ToList();
        tmp.Remove(battleParticipant);
        turnOrder.Clear();
        foreach(CombatParticipantData p in tmp)
        {
            turnOrder.Enqueue(p);
        }
        OnBattleStateChanged?.Invoke(this);
    }

    public void EndTurn()
    {
        turnOrder.Enqueue(turnOrder.Dequeue());
        OnBattleStateChanged?.Invoke(this);
    }
    public void CompleteBattle()
    {
 
        playerTeam.Clear();
        enemyTeam.Clear();
    }

    public bool IsPlayerTeamAlive()
    {
        return playerTeam.Any(p => p.currentHP > 0);
    }

    public bool IsEnemyTeamAlive()
    {

        return enemyTeam.Any(e => e.currentHP > 0);
    }

    public bool IsPlayer(CombatParticipantData p)
    {
        return playerTeam.Contains(p);
    }

    public CombatParticipantData GetTurnPlayer() { return turnPlayer; }
}
