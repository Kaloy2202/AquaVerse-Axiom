using UnityEngine;
public class QuestObjective
{
    public string ObjectiveDescription { get; private set; }
    public bool IsCompleted { get; private set; }

    public QuestObjective(string description)
    {
        ObjectiveDescription = description;
        IsCompleted = false;
    }

    public void CompleteObjective()
    {
        IsCompleted = true;
        Debug.Log($"Objective '{ObjectiveDescription}' completed!");
    }
}
