using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChallengeUI : MonoBehaviour
{
    public Text challengeText;
    public ChallengeManager challengeManager;

    void Update()
    {
        challengeText.text = ""; // Clear text
        foreach (Challenge challenge in challengeManager.activeChallenges)
        {
            challengeText.text += $"{challenge.challengeName}: {challenge.currentProgress}/{challenge.requiredAmount}\n";
        }
    }
}

