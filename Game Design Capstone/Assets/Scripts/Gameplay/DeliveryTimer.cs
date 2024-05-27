using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class DeliveryTimer : MonoBehaviour
{
    [SerializeField] private float duration = 100f;

    private float timeRemaining, firstPhaseDuration, secondPhaseDuration, elapsedTime, startTime;

    public bool isRunning;

    void Start()
    {
        // calculate phase durations
        firstPhaseDuration = duration * 0.75f;
        secondPhaseDuration = duration * 0.25f;
    }

    void Update()
    {
        if (isRunning)
        {
            timeRemaining -= Time.deltaTime;
            elapsedTime = Time.time - startTime;

            // stop the timer if there is no time left
            if (timeRemaining <= 0f)
            {
                timeRemaining = 0f;
                isRunning = false;
            }

            // update the UI displays
            UIManager.instance.UpdateStopwatchDisplay((int)elapsedTime);
            UIManager.instance.UpdateTempDisplay((int)elapsedTime, firstPhaseDuration, secondPhaseDuration);
        }
        else
        {
            if (startTime != 0)
                elapsedTime = Time.time - startTime;
        }
    }

    public void StartTimer()
    {
        timeRemaining = duration;
        isRunning = true;
        startTime = Time.time;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        timeRemaining = duration;
        elapsedTime = 0;
        startTime = 0;
        isRunning = false;
    }

    public int GetElapsedTime() { return (int)elapsedTime; }
    public float GetTimerDuration() { return duration; }
    public float GetCurrentTemp() { return duration - elapsedTime; }
}

