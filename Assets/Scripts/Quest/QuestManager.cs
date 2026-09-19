using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<QuestSO> quests;
    [ShowInInspector]
    private Dictionary<QuestSO, QuestData> questLibrary = new();
    public static QuestManager instance;
    public static event Action<QuestData> onQuestUpdate;

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

    [ContextMenu("Init")]
   public void InitializeQuestLibrary()
    {
        foreach(QuestSO q in quests)
        {
            QuestData tmp = new QuestData(q);
            tmp.onQuestUpdated += UpdateQuest;
            tmp.onQuestCompleted += CompleteQuest;
            questLibrary.Add(q, tmp);
        }
        ActivateQuest(quests[0]);
    }

    public void UpdateQuest(QuestData quest)
    {
        questLibrary[quest.config] = quest;
        onQuestUpdate?.Invoke(quest);
    }

    public void CompleteQuest(QuestData quest)
    {
        questLibrary[quest.config].isComplete = true;

        // initiate rewards
    }

    public void ActivateQuest(QuestSO quest)
    {
        questLibrary[quest].isActive = true;
        GoalManager.instance.TrackQuest(questLibrary[quest]);
    }
    







}
