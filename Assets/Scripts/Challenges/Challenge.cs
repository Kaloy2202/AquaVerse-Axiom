using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Challenge
{
    public string challengeName;
    public string description;
    public int requiredAmount;
    public int currentProgress;
    public int experienceReward;
    public int moneyReward;
    public bool isCompleted;

    public Challenge(string name, string desc, int required, int xpReward, int moneyReward, RewardPopupManager popupManager)
    {
        challengeName = name;
        description = desc;
        requiredAmount = required;
        experienceReward = xpReward;
        this.moneyReward = moneyReward;
        currentProgress = 0;
        isCompleted = false;
    }

    public void CheckIfCompleted(PlayerStats playerStats)
    {
        if (currentProgress >= requiredAmount)
        {
            isCompleted = true;
            GiveReward(playerStats);
        }
    }

    private void GiveReward(PlayerStats playerStats)
    {
        Debug.Log("Challenge completed: " + challengeName);
        playerStats.GainExperience(experienceReward); // Reward experience
        playerStats.AddMoney(moneyReward); // Reward money

        UnlockNewTool();
    }

    private void UnlockNewTool()
    {
        // Logic to unlock a new tool, bonus item, or feature
        Debug.Log("New tool unlocked!");
    }


}

