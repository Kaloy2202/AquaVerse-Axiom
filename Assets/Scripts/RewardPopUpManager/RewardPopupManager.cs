using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RewardPopupManager : MonoBehaviour
{
    public GameObject rewardPopup; // The popup UI element
    public TextMeshProUGUI rewardText; // Text for the reward message
    public Image lv2;
    public Image lv3;
    public Image lv4;
    public Image lv5;
    public Image lv6;
    public Image lv10;
    public Image lv15;
    public Image badge1;
    public Image badge2;
    public Image badge3;
    public Image badge4;
    public Image placeHolder;


    //void Start()
    //{
    //    ShowRewardPopup("Test: Popup is working!");
    //}

    // Method to show the popup
    public void ShowRewardPopup(string rewardMessage, int currentLvl)
    {
        rewardPopup.SetActive(true); // Enable the popup UI
        // rewardText.text = rewardMessage; // Set the reward message text

        if (currentLvl == 2)
        {
            lv2.gameObject.SetActive(true);
            badge1.gameObject.SetActive(true);
            placeHolder.gameObject.SetActive(false);
        }
        else if (currentLvl == 3)
        {
            lv3.gameObject.SetActive(true);
        }
        else if (currentLvl == 4)
        {
            lv4.gameObject.SetActive(true);
        }
        else if (currentLvl == 5)
        {
            lv5.gameObject.SetActive(true);
            badge2.gameObject.SetActive(true);
            badge1.gameObject.SetActive(false);
        }
        else if (currentLvl == 6)
        {
            lv6.gameObject.SetActive(true);
        }
        else if (currentLvl == 10)
        {
            lv10.gameObject.SetActive(true);
            badge3.gameObject.SetActive(true);
            badge2.gameObject.SetActive(false);
        }
        else if (currentLvl == 15)
        {
            lv15.gameObject.SetActive(true);
            badge4.gameObject.SetActive(true);
            badge3.gameObject.SetActive(false);
        }
        // Hide the popup after 3 seconds
        Invoke("HideRewardPopup", 3f); // Change duration if needed
    }

    // Method to hide the popup
    void HideRewardPopup()
    {
        rewardPopup.SetActive(false); // Disable the popup UI
    }
    
}

