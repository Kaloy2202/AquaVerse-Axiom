using UnityEngine;
using TMPro;
using System;

public class CustomTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float secondsPerDay = 30f;
    private float timer = 0f;
    private int days = 0;
    private int hours = 0;
    private int months = 0;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= secondsPerDay)
        {
            days++;
            timer -= secondsPerDay;

            if (days >= 30)
            {
                months++;
                days = 0;
            }
        }

        hours = Mathf.FloorToInt((timer / secondsPerDay) * 24);

        UpdateTimerDisplay();
    }

    void UpdateTimerDisplay()
    {
        timerText.text = string.Format("{0:00} M: {1:00} D: {2:00} H", months, days, hours);
    }
}