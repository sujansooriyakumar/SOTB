using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class GoalData
{
    [ShowInInspector] public Dictionary<RequirementSO, RequirementData> requirements = new();
    public GoalSO config;
    public bool isActive;
    public bool isComplete;
    public int goalID;
    public int nextGoalID;
    public event Action<GoalData> onGoalComplete;
    public event Action<GoalData> onGoalUpdated;

    public GoalData(GoalSO config_)
    {
        config = config_;
        isActive = false;
        goalID = config.goalID;
        nextGoalID = config.nextGoalID;
        foreach(RequirementSO req in config.requirements)
        {
            RequirementData tmp = req.CreateRuntimeData();
            tmp.onRequirementUpdated += HandleRequirementChange;
            requirements.Add(req, tmp);
            
        }
    }

    public bool IsCompleted()
    {
        isComplete = requirements.Values.All(r => r.IsMet());
        if (isComplete) onGoalComplete?.Invoke(this);
        return isComplete;
    }


    private void HandleRequirementChange(RequirementData req)
    {
        requirements[req.Config] = req;

        onGoalUpdated?.Invoke(this); // UI / data refresh

        if (isActive && IsCompleted())
        {
            onGoalComplete?.Invoke(this); // Only on actual completion
        }
    }

    public void ActivateGoal()
    {
        isActive = true;
        onGoalUpdated?.Invoke(this);
    }
}
