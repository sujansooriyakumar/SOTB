using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceModEffectSO", menuName = "SOTB/Effects/StatModEffectSO")]

public class StatModEffectSO: EffectSO
{
    public CombatStat stat;
    public int stage;

    public override void ExecuteEffect(CombatParticipantData target)
    {
        float modifier = stage * 0.25f;
        float statRef = target.stats.statLibrary[stat];
        statRef = statRef * modifier;
        target.stats.statLibrary[stat] = (int)Mathf.CeilToInt(statRef);
    
    }
}
