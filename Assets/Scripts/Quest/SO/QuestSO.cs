using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "QuestSO", menuName = "Quest System/QuestSO")]
public class QuestSO : ScriptableObject
{
    public string questName;
    public List<GoalSO> goals;
    public RewardSO reward;
    public int initialGoalID;
}
