using System.Collections.Generic;
using UnityEngine;

public class ChallengeManager : MonoBehaviour
{
    public List<Challenge> activeChallenges = new List<Challenge>();
    private PlayerStats playerStats;
    public RewardPopupManager rewardPopupManager; // Reference to the RewardPopupManager

    void Start()
    {
        // Find the PlayerStats script on the player (or wherever it's attached)
        playerStats = FindObjectOfType<PlayerStats>();

        // Example of adding challenges
        activeChallenges.Add(new Challenge("Feed 10 Fish", "Feed your fish 10 times", 10, 100, 500, rewardPopupManager)); // Pass RewardPopupManager here
    }

    // Call this to update challenge progress
    public void UpdateChallengeProgress(string challengeName, int progress)
    {
        foreach (Challenge challenge in activeChallenges)
        {
            if (challenge.challengeName == challengeName && !challenge.isCompleted)
            {
                challenge.currentProgress += progress;
                challenge.CheckIfCompleted(playerStats); // Pass PlayerStats for rewards
            }
        }
    }
}
