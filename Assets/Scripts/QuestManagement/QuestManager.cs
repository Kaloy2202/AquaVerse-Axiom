using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<Quest> quests = new List<Quest>();

    public void AddQuest(Quest newQuest)
    {
        quests.Add(newQuest);
        Debug.Log($"New quest added: {newQuest.QuestName}");
    }

    public void CompleteObjective(Quest quest, QuestObjective objective)
    {
        if (quest.Objectives.Contains(objective))
        {
            objective.CompleteObjective();
            quest.CheckQuestCompletion();
        }
    }
}
