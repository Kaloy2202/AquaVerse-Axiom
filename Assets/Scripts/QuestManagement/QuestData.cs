using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestData
{
    public string questId;
    public string[] dialogueLines;
    public int requiredProgress;
    public bool isCompleted;
    public string questDescription;
}