using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EscapeCombatAction : CombatAction
{
    public override event Action OnActionComplete;

    private List<CombatParticipantData> enemyTeam;
    private CombatParticipantData turnPlayer;

    public EscapeCombatAction(CombatParticipantData turnPlayer_, List<CombatParticipantData> enemyTeam_)
    {
        this.enemyTeam = enemyTeam_;
        this.turnPlayer = turnPlayer_;
    }

    public override void ExecuteAction()
    {
        float enemyAGI = 0f;
        float playerAGI = turnPlayer.stats.GetStat(CombatStat.AGI);
        foreach (CombatParticipantData enemy in enemyTeam)
        {
            enemyAGI += enemy.stats.GetStat(CombatStat.AGI);
        }
        enemyAGI /= enemyTeam.Count;

        float odds = 1.0f - (playerAGI / enemyAGI);
        float rng = UnityEngine.Random.value;
        if (rng <= odds)
        {
            // fail escape logic
        }
        else if(rng > odds || playerAGI > enemyAGI) 
        {
            // escape logic
        }
        OnActionComplete?.Invoke();

    }
}
