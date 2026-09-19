using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GoalSO", menuName = "Quest System/GoalSO")]
public class GoalSO : ScriptableObject
{
    public List<RequirementSO> requirements;
    public int goalID;
    public int nextGoalID;
}
