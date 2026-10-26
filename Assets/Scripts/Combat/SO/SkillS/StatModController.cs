using UnityEngine;

public class StatModController
{
    private CombatParticipantData owner;
    private StatMod statMod;
    private float baseVal;
    private int remainingTurns;

    public StatModController(CombatParticipantData owner_)
    {
        owner = owner_;
    }

    public void InitializeStatMod(StatMod statMod_)
    {
        statMod = statMod_;
        baseVal = owner.stats.GetStat(statMod.stat);
        int tmp = Mathf.CeilToInt(baseVal * statMod.mod);
        owner.stats.SetStat(statMod.stat, tmp);
        remainingTurns = statMod.duration;
        BattleManager.Instance.OnTurnComplete += UpdateDuration;
        BattleManager.Instance.OnBattleComplete += Revert;
    }

    private void UpdateDuration(CombatParticipantData player)
    {
        if (player != owner) return;
        if(--remainingTurns <= 0)
        {
            Revert();
            
        }
    }

    private void Revert()
    {
        owner.stats.SetStat(statMod.stat, Mathf.CeilToInt(baseVal));
        BattleManager.Instance.OnTurnComplete -= UpdateDuration;
        BattleManager.Instance.OnBattleComplete -= Revert;
    }
}
