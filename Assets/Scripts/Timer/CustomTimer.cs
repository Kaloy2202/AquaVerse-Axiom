using UnityEngine;
using TMPro;
using System;
using System.Collections;

public class CustomTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private int startingHour = 8;
    private float secondsPerHour;
    private float timer = 0f;
    private int days = 0;
    private int hours = 0;
    private int months = 0;

    private SceneMngrState sceneMngrState;

    void Start(){
        sceneMngrState = GameObject.Find("SceneManager").GetComponent<SceneMngrState>();
        secondsPerHour = sceneMngrState.getNumberOfSecondsPerHour();
        sceneMngrState.setGameTime(startingHour);
        sceneMngrState.setStartingTime(startingHour);
        hours = startingHour;
        UpdateTimerDisplay();
        StartCoroutine(calculateTime());
    }

    IEnumerator calculateTime(){
        while (true){
            yield return new WaitForSeconds(secondsPerHour);
            hours ++;
            if(hours == 24){
                hours =0;
                days ++;
                if(days == 30){
                    months ++;
                    days = 0;
                }
            }
            sceneMngrState.setGameTime(hours);
            UpdateTimerDisplay();
        }
    }

    void UpdateTimerDisplay()
    {
        timerText.text = string.Format("{0:00} M: {1:00} D: {2:00} H", months, days, hours);
    }
}