using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDisappear : MonoBehaviour
{
    // Reference to the Canvas GameObject
    public GameObject canvas;
    public string questName;
    public int experienceReward;
    public int moneyReward;
    public int progressReward;

    // Update is called once per frame
    void Update()
    {
        // Check for left mouse click (0 is the left mouse button)
        if (Input.GetMouseButtonDown(0) && QuestManager.Instance.IsQuestComplete(questName) == false)
        {
            
            // Disable the canvas by setting it to inactive
            QuestManager.Instance.UpdateQuestProgress(questName, progressReward);
            PlayerStats.Instance.GainExperience(experienceReward);
            PlayerStats.Instance.money += moneyReward;
            canvas.SetActive(false);
            
        }
    }
}
