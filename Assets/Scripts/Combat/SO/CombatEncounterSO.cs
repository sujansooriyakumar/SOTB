using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CombatEncounterSO", menuName = "Scriptable Objects/CombatEncounterSO")]
public class CombatEncounterSO : ScriptableObject
{
    public List<CombatParticipantSO> enemies;
    public int expReward;
    public int moneyReward;
    // item rewards? or maybe attached to enemies?
}
