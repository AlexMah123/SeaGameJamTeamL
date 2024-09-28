using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Timer : MonoBehaviour
{
    public float timeLimit = 180f; // 3 minutes (180 seconds)
    private float timeRemaining;
    public static bool timerIsRunning = false;
    public TMP_Text timeText;

    // Start time at 10:00 PM
    private int currentHour = 10;
    private string period = "PM";

    void Start()
    {
        // Initialize the timer
        timeRemaining = timeLimit;
        timerIsRunning = true;
        DisplayTime(currentHour); // Show initial time (10 PM)
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;

                // Check if a whole hour has passed
                if (timeRemaining <= timeLimit - 30f) // 60 seconds = 1 minute (simulating 1 hour)
                {
                    IncrementHour();
                }
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                EndTimer();
            }
        }
    }

    // Method to increment the hour and adjust AM/PM
    public void IncrementHour()
    {
        timeLimit -= 30f; // Reduce time limit for next hour increment
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

        // Update the display with the new hour
        DisplayTime(currentHour);
    }

    // Method to display the hour with AM/PM
    void DisplayTime(int hour)
    {
        string formattedTime = string.Format("{0}:00 {1}", hour, period);
        timeText.text = formattedTime;
    }

    void EndTimer()
    {
        Debug.Log("Time has run out!");
        // Trigger any game over logic here
    }
}