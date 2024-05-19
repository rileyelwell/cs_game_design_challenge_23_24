using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeliveryTimer : MonoBehaviour
{
    [SerializeField] private float duration = 59f;
    [SerializeField] private bool autoStart = true;

    [SerializeField] private TMPro.TextMeshProUGUI timerText;

    private float timeRemaining;
    private bool isRunning;

    void Start()
    {
        if (autoStart)
            StartTimer();
    }

    void Update()
    {
        if (isRunning)
        {
            timeRemaining -= Time.deltaTime;

            // stop the timer if there is no time left
            if (timeRemaining <= 0f)
            {
                timeRemaining = 0f;
                isRunning = false;
            }

            // update the UI with the current time
            UpdateTimerUI((int)timeRemaining);
        }
    }

    private void StartTimer()
    {
        timeRemaining = duration;
        isRunning = true;
    }

    private void StopTimer()
    {
        isRunning = false;
    }

    private void ResetTimer()
    {
        timeRemaining = duration;
        isRunning = false;
    }

    private void UpdateTimerUI(int currentTime)
    {
        if (currentTime < 10)
        {
            timerText.text = $"0:0{currentTime}";
        } else 
        {
            timerText.text = $"0:{currentTime}";
        }
    }
}
