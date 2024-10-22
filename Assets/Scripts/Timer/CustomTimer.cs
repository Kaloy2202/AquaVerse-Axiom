using UnityEngine;
using TMPro;
using System;
using System.Collections;

public class CustomTimer : MonoBehaviour
{
    [SerializeField] private int startingTime;
    public TextMeshProUGUI timerText;
    private int days = 0;
    private int hours = 0;
    private int months = 0;

    private SceneMngrState sceneMngrState;
    private float secondsPerHour;

    void Start(){
        sceneMngrState = GameObject.Find("SceneManager").GetComponent<SceneMngrState>();
        secondsPerHour = sceneMngrState.getSecondsPerHour();
        sceneMngrState.setHour(startingTime);
        hours = startingTime;
        StartCoroutine(calcGameTime());
    }

    IEnumerator calcGameTime(){
        while(true){
            yield return new WaitForSeconds(secondsPerHour);
            hours += 1;
            sceneMngrState.setHour(hours);
            if (hours == 24)
            {
                hours = 0;
                days++;

                if (days >= 30)
                {
                    months++;
                    days = 0;
                }
            }
            UpdateTimerDisplay();

        }
    }
    void UpdateTimerDisplay()
    {
        timerText.text = string.Format("{0:00} M: {1:00} D: {2:00} H", months, days, hours-startingTime);
    }
}