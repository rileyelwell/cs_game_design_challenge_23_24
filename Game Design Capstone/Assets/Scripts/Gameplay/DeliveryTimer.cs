using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class DeliveryTimer : MonoBehaviour
{
    [SerializeField] private float duration = 100f;         // How long the timer will be
    private float elapsedTime;                              // Time passed on timer
    private float timeRemaining;                            // Time left on the timer
    private float startTime;                                // Time at timer start
    private float firstPhaseDuration, secondPhaseDuration;  // Phases for temperature UI
    public bool isRunning;                                  // Boolean for if the timer is on

    /*
     * Name: Start (Unity)
     * Inputs: none
     * Outputs: none
     * Description: Calculates phase durations
     */
    void Start()
    {
        firstPhaseDuration = duration * 0.75f;
        secondPhaseDuration = duration * 0.25f;
    }

    /*
     * Name: Update (Unity)
     * Inputs: none
     * Outputs: none
     * Description: Handles the timer and the clock and temperature UI
     */
    void Update()
    {
        if (isRunning)
        {
            timeRemaining -= Time.deltaTime;
            elapsedTime = Time.time - startTime;

            // Stop the timer if there is no time left
            if (timeRemaining <= 0f)
            {
                timeRemaining = 0f;
                isRunning = false;
            }

            // Update the UI displays
            UIManager.instance.UpdateStopwatchDisplay((int)elapsedTime);
            UIManager.instance.UpdateTempDisplay((int)elapsedTime, firstPhaseDuration, secondPhaseDuration);
        }
        else
        {
            if (startTime != 0)
                elapsedTime = Time.time - startTime;
        }
    }

    /*
     * Name: StartTimer
     * Inputs: none
     * Outputs: none
     * Description: Starts the timer
     */
    public void StartTimer()
    {
        timeRemaining = duration;
        isRunning = true;
        startTime = Time.time;
    }

    /*
     * Name: StopTimer
     * Inputs: none
     * Outputs: none
     * Description: Stops the timer
     */
    public void StopTimer()
    {
        isRunning = false;
    }

    /*
     * Name: ResetTimer
     * Inputs: none
     * Outputs: none
     * Description: Resets and stops the timer
     */
    public void ResetTimer()
    {
        timeRemaining = duration;
        elapsedTime = 0;
        startTime = 0;
        isRunning = false;
    }

    /*
     * Name: GetElapsedTime
     * Inputs: none
     * Outputs: integer of elapsed time
     * Description: Gets the elapsed time
     */
    public int GetElapsedTime()
    {
        return (int)elapsedTime;
    }

    /*
     * Name: GetTiemrDuration
     * Inputs: none
     * Outputs: float of timer duration
     * Description: Gets the timer duration
     */
    public float GetTimerDuration()
    {
        return duration;
    }

    /*
     * Name: GetCurrentTemp
     * Inputs: none
     * Outputs: float of current temp
     * Description: Gets the current temp
     */
    public float GetCurrentTemp()
    {
        return duration - elapsedTime;
    }
}

