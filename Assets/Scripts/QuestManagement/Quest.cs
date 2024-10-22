using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public string QuestName { get; private set; }
    public string Description { get; private set; }
    public List<QuestObjective> Objectives { get; private set; }
    public bool IsCompleted { get; private set; }
    public int Reward { get; private set; }

    public Quest(string name, string description, int reward, List<QuestObjective> objectives)
    {
        QuestName = name;
        Description = description;
        Reward = reward;
        Objectives = objectives;
    }

    public void CheckQuestCompletion()
    {
        // Check if all objectives are complete
        IsCompleted = true;
        foreach (var objective in Objectives)
        {
            if (!objective.IsCompleted)
            {
                IsCompleted = false;
                break;
            }
        }

        if (IsCompleted)
        {
            // Grant rewards to the player
            Debug.Log($"Quest '{QuestName}' is complete! Reward: {Reward} points.");
        }
    }
}
