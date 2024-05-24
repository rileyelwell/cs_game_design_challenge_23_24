using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHandler : MonoBehaviour
{
    public static ScoreHandler instance;

    [SerializeField] private GameObject scorePanel, gameplayPanel;
    [SerializeField] private TMPro.TextMeshProUGUI scoreDescText, elapsedTimeText, scoreTitleText;
    [SerializeField] private Image star1, star2, star3, timerImage;

    [SerializeField] private Image robotHealthBar, tempBarRectangle, tempBarCircle;

    [SerializeField] private GameObject deliveryHandler;


    // [SerializeField] private int oneStarThreshold = 100, twoStarThreshold = 200, threeStarThreshold = 300; 

    // keeping hex code for color reference
    // yellow -> FFC600
    // beaver orange -> D73F09
    // light yellow -> FEFFC4

    private void Awake() {
        // create a singleton instance of gameplay manager to exist during this execution
        if (instance != null && instance != this) { 
            Destroy(this); 
        } else { 
            instance = this; 
        }
    }

    private void Update() {
        CheckLoseConditions();
    }

    public void SetRobotHealth(float healthLost)
    {
        if (robotHealthBar.fillAmount - healthLost >= 0)
        {
            robotHealthBar.fillAmount -= healthLost;
        } else {
            robotHealthBar.fillAmount = 0;
        }
    }

    public void SetFoodTempRect(float temp)
    {
        // print($"First Temp: {temp}");
        if (1f - temp >= 0.02)
        {
            tempBarRectangle.fillAmount = 1f - temp;
        } else {
            tempBarRectangle.fillAmount = 0;
        }
    }

    public void SetFoodTempCircle(float temp)
    {
        // print($"Second Temp: {temp}");
        if (1f - temp >= 0.02)
        {
            tempBarCircle.fillAmount = 1f - temp;
        } else {
            tempBarCircle.fillAmount = 0;
        }
    }

    public void CalculateDeliveryScore()
    {
        int time = gameObject.GetComponent<DeliveryTimer>().GetElapsedTime();
        float tempDuration = gameObject.GetComponent<DeliveryTimer>().GetTimerDuration();
        float temp = gameObject.GetComponent<DeliveryTimer>().GetCurrentTemp() / tempDuration;
        float health = robotHealthBar.fillAmount;

        Debug.Log($"Time: {time}, Temp: {temp}, Health: {health}");

        // display the time taken on the scoreboard  
        elapsedTimeText.enabled = true;
        timerImage.enabled = true;
        if (time < 10)
        {
            elapsedTimeText.text = $"0:0{time}";
        } else 
        {
            elapsedTimeText.text = $"0:{time}";
        }
            
        scoreTitleText.text = "Delivery Complete!";

        // user receives 3 stars for perfect health and at least half the temp gauge left full
        if (health == 1 && temp >= 0.5) 
        {
            star1.color = Color.white;
            star2.color = Color.white;
            star3.color = Color.white;
            scoreDescText.text = "Excellent Delivery!";
        } 
        // user receives 2 stars for perfect health and poor temp, or good temp and decent health
        else if ((health == 1 && temp < 0.5 && temp > 0) || temp >= 0.5 && health >= 0.5)
        {
            star1.color = Color.white;
            star2.color = Color.white;
            star3.color = Color.black;
            scoreDescText.text = "Good Delivery!";
        }
        // user receives 1 star for poor health and poor temp, or no temp and perfect health
        else if (health <= 0.5 && health > 0 && temp < 0.5 && temp > 0)
        {
            star1.color = Color.white;
            star2.color = Color.black;
            star3.color = Color.black;
            scoreDescText.text = "Poor Delivery!";
        }
        // user receives no stars for anything worse 
        else 
        {
            star1.color = Color.black;
            star2.color = Color.black;
            star3.color = Color.black;
            scoreDescText.text = "Delivery Failed!";
        }
    }

    private void DisplayFailedDeliveryyScore()
    {
        // stop the game's movements
        Time.timeScale = 0f;

        // calculate the score for the delivery finished and display to user
        CalculateDeliveryScore();
        scorePanel.SetActive(true);
        gameplayPanel.SetActive(false);
        GameplayManager.instance.canPause = false;

        
    }

    public void DisplayScoreScreen()
    {
        // stop the game's movements
        Time.timeScale = 0f;

        // calculate the score for the delivery finished and display to user
        // CalculateDeliveryScore();
        scorePanel.SetActive(true);
        gameplayPanel.SetActive(false);
        GameplayManager.instance.canPause = false;

        // reset the temp and health bars
        ResetUI();
    }

    private void CheckLoseConditions()
    {
        // print(tempBarCircle.fillAmount);
        // if the player has lost all health or food temp has dropped to min, end current delivery
        if (robotHealthBar.fillAmount == 0 || tempBarCircle.fillAmount <= 0)
        {
            // calculate the failed delivery
            CalculateDeliveryScoreFailed(tempBarCircle.fillAmount, robotHealthBar.fillAmount);

            // update some variables for deliveries & display
            deliveryHandler.GetComponent<DeliveryHandler>().UpdateForFailedDelivery();
        }
    }

    public void ResetUI() 
    {
        SetFoodTempCircle(0f);
        SetFoodTempRect(0f);
        SetRobotHealth(-1f);
    }

    private void CalculateDeliveryScoreFailed(float temp, float health)
    {
        elapsedTimeText.enabled = false;
        timerImage.enabled = false;

        // zero stars awarded and delivery failed text
        star1.color = Color.black;
        star2.color = Color.black;
        star3.color = Color.black;

        scoreTitleText.text = "Delivery Failed!";

        if (temp <= 0.01 && health > 0) {
            scoreDescText.text = "Your food is too cold!";
        }

        else if (health <= 0 && temp > 0.1f)
        {
            scoreDescText.text = "Your robot has been badly damaged!";
        }

        else 
        {
            scoreDescText.text = "Your robot has been badly damaged and your food is cold!";
        }
        
    }
}
