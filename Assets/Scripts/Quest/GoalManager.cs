using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine.Events;

public class GoalManager : MonoBehaviour
{
    [ShowInInspector] public Dictionary<GoalSO, GoalData> goalLibrary = new();
    public static GoalManager instance;
    public Action<GoalData> onGoalComplete;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject); // if needed
    }

    public void SetBoolRequirement(BoolRequirementSO requirement, bool newValue)
    {
        
        foreach(var goalData in goalLibrary.Values)
        {
            if (goalData.isActive == false) continue;
            if (goalData.requirements.TryGetValue(requirement, out RequirementData baseData) &&
    baseData is BoolRequirementData reqData)
            {
                reqData.value = newValue;
                // call update goal here
            }

        }
    }

    public void IncrementIntRequirement(IntRequirementSO requirement, int increment)
    {
        foreach (var goalData in goalLibrary.Values)
        {
            if (goalData.isActive == false) continue;
            if (goalData.requirements.TryGetValue(requirement, out RequirementData baseData) &&
               baseData is IntRequirementData reqData)
            {
                reqData.Increment(increment);
                
            }
        }
    }



    public void UpdateGoal(GoalData goal)
    {
        if(goal.isActive && goal.IsCompleted())
        {
            // completed goal logic
            if (goal.nextGoalID > -1)
            {
                ActivateGoal(goal.nextGoalID);
            }
            goal.isActive = false;
            onGoalComplete?.Invoke(goal);
            
        }
    }

    public void ActivateGoal(int goalID)
    {
        foreach(GoalData goal in goalLibrary.Values)
        {
            goal.onGoalUpdated += UpdateGoal;
            if(goalID == goal.goalID)
            {
                goal.ActivateGoal();
            }
        }
    }

    public void TrackQuest(QuestData quest)
    {
        goalLibrary.AddRange(quest.goals);
        ActivateGoal(quest.initialGoalID);
    }
}
