using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public float timeLimitSeconds = 180f; 
    public float timeSecondsPerHour = 30f;
    public float timeRemaining;
    public static bool timerIsRunning = false;
    public TMP_Text timeText;

    // Start time at 10:00 PM
    private int currentHour = 10;
    private string period = "PM";

    void Start()
    {
        timeRemaining = timeLimitSeconds;
        timerIsRunning = true;
        DisplayTime(currentHour);
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;

                // Check if a whole hour has passed
                if (timeRemaining <= timeLimitSeconds - timeSecondsPerHour)
                {
                    IncrementHour();
                }
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

    public void IncrementHour()
    {
        timeLimitSeconds -= timeSecondsPerHour; // Reduce time limit for next hour increment
        currentHour++;

        // Handle transition from PM to AM and the 12-hour format
        if (currentHour == 12)
        {
            // Switch from AM to PM or vice versa at 12
            period = (period == "PM") ? "AM" : "PM";
        }
        else if (currentHour > 12)
        {
            // Wrap around the hour after 12 to 1
            currentHour = 1;
        }

        DisplayTime(currentHour);
    }

    void DisplayTime(int hour)
    {
        string formattedTime = string.Format("{0}:00 {1}", hour, period);
        timeText.text = formattedTime;
    }
}