using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RewardPopupManager : MonoBehaviour
{
    public GameObject rewardPopup; // The popup UI element
    public TextMeshProUGUI rewardText; // Text for the reward message

    //void Start()
    //{
    //    ShowRewardPopup("Test: Popup is working!");
    //}

    // Method to show the popup
    public void ShowRewardPopup(string rewardMessage)
    {
        rewardPopup.SetActive(true); // Enable the popup UI
        rewardText.text = rewardMessage; // Set the reward message text

        // Hide the popup after 3 seconds
        Invoke("HideRewardPopup", 3f); // Change duration if needed
    }

    // Method to hide the popup
    void HideRewardPopup()
    {
        rewardPopup.SetActive(false); // Disable the popup UI
    }
}

