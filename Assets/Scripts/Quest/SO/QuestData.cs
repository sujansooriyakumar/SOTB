using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestData
{
    public string questName;
    [ShowInInspector] public Dictionary<GoalSO, GoalData> goals = new();
    [ShowInInspector] public RewardData reward;
    public event Action<QuestData> onQuestUpdated;
    public event Action<QuestData> onQuestCompleted;
    public bool isComplete = false;
    public bool isActive = false;
    public QuestSO config;
    public int initialGoalID = 0;
    public int completedGoals;

    public QuestData(QuestSO config_)
    {
        config = config_;
        completedGoals = 0;
        initialGoalID = config.initialGoalID;
        questName = config.questName;
        foreach(GoalSO goalConfig in config.goals)
        {
            GoalData tmp = new GoalData(goalConfig);
            goals.Add(goalConfig, tmp);
            tmp.onGoalComplete += GoalComplete;
            tmp.onGoalUpdated += GoalUpdated;
            

        }

    }

    public void GoalComplete(GoalData goal)
    {
        completedGoals++;
        if(completedGoals >= goals.Values.Count)
        {
            onQuestCompleted?.Invoke(this);
        }

    }

    public void GoalUpdated(GoalData goal)
    {
        goals[goal.config] = goal;
        onQuestUpdated?.Invoke(this);
    }

    public void ActivateQuest()
    {
        isActive = true;
        onQuestUpdated?.Invoke(this);

    }

    public void ActivateGoal(GoalSO goal)
    {
        goals[goal].ActivateGoal();
    }

   
}
