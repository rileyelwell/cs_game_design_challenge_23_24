using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class DeliveryTimer : MonoBehaviour
{
    [SerializeField] private float duration = 100f;
    // [SerializeField] private bool autoStart = false;

    [SerializeField] private TMPro.TextMeshProUGUI timerText;

    private float timeRemaining, firstPhaseDuration, secondPhaseDuration, elapsedTime;
    private float startTime;
    public bool isRunning;

    void Start()
    {
        // Calculate phase durations
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

            // update the UI with the current time
            //UpdateTimerUI((int)timeRemaining);

            // update the first fill image for the first phase duration
            if (elapsedTime <= firstPhaseDuration)
            {
                ScoreHandler.instance.SetFoodTempRect(elapsedTime / firstPhaseDuration);
            }

            // update the second image for the second phase duration
            else
            {
                float secondPhaseElapsed = elapsedTime - firstPhaseDuration;
                ScoreHandler.instance.SetFoodTempCircle(secondPhaseElapsed / secondPhaseDuration);
            }
        }
        else
        {
            if (startTime != 0) // Ensure startTime is set
            {
                elapsedTime = Time.time - startTime;
            }
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

    public int GetElapsedTime() 
    {
        return (int)elapsedTime;
    }

    public float GetTimerDuration() 
    {
        return duration;
    }

    public float GetCurrentTemp() 
    {
        return duration - elapsedTime; 
    }

    private void UpdateTimerUI(int currentTime)
    {
        if (currentTime < 10)
        {
            timerText.text = $"0:0{currentTime}";
        } 
        else 
        {
            timerText.text = $"0:{currentTime}";
        }
    }
}

