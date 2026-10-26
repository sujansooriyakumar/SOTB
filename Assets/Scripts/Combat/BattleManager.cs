using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private Queue<CombatParticipantData> turnOrder;

    public static BattleManager Instance;

    private List<CombatParticipantData> playerTeam;
    private List<CombatParticipantData> enemyTeam = new();
    public List<CombatParticipantData> battleParticipants= new();
    public event Action<CombatParticipantData> OnTurnComplete;
    public event Action OnBattleComplete;
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
        foreach(CombatParticipantSO config in encounter_.enemies)
        {
            enemyTeam.Add(new CombatParticipantData(config));
        }
        playerTeam = PartyManager.Instance.partyMembers;
        battleParticipants = playerTeam.Concat(enemyTeam).ToList();
        battleParticipants.OrderBy(tmp => tmp.stats.stats[CombatStat.AGI]);

       
        
    }

    private IEnumerator BattleCoroutine()
    {
        // while enemy team list or palyer team list all have hp > 0
        yield return null;
    }
}
