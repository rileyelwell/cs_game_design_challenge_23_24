using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // singleton instance
    public static UIManager instance;

    // reference to some needed scripts
    private DeliveryTimer deliveryTimer;
    private DeliveryHandler deliveryHandler;
    private GameObject player;

    // for sensors
    [SerializeField] private Image leftSensorImg, rightSensorImg, backSensorImg, frontSensorImg; 

    // for health bar
    [SerializeField] private Image robotHealthImage;

    // for temperature bar
    [SerializeField] private Image tempRectangleImage, tempCircleImage;

    // for battery bar
    [SerializeField] private Image batteryImage;
    [SerializeField] private float batteryCooldownLimit = 0.5f;
    public bool isBatteryOnCooldown;

    // for timer & stopwatch
    [SerializeField] private TMPro.TextMeshProUGUI stopwatchText;

    // for current objective
    [SerializeField] private TMPro.TextMeshProUGUI currObjText;

    // for scoreboard
    [SerializeField] private TMPro.TextMeshProUGUI scoreDescText, elapsedTimeText, scoreTitleText;
    [SerializeField] private Image star1, star2, star3, timerImage;

    // for arrow pointer
    [SerializeField] private RectTransform arrowImage;
    [SerializeField] private float rotationSpeed = 5f; 
    private Transform targetWaypoint;
       


    private void Awake() {
        // create a singleton instance of UI manager to exist during this execution
        if (instance != null && instance != this)
            Destroy(this); 
        else 
            instance = this; 

        deliveryTimer = gameObject.GetComponent<DeliveryTimer>();
        deliveryHandler = GameObject.Find("DeliveryHandler").GetComponent<DeliveryHandler>();
        player = GameObject.FindGameObjectWithTag(TagManager.PLAYER_TAG);
    }

    public void UpdateSensorDisplay(bool front, bool back, bool left, bool right) {
        // if there is a signal from ADAS, display red outline, otherwise keep greyed out
        frontSensorImg.color = front ? Color.red : Color.gray;
        backSensorImg.color = back ? Color.red : Color.gray;
        leftSensorImg.color = left ? Color.red : Color.gray;
        rightSensorImg.color = right ? Color.red : Color.gray;
    }

    public void UpdateRobotHealthDisplay(float healthLost)
    {
        if (robotHealthImage.fillAmount - healthLost >= 0)
            robotHealthImage.fillAmount -= healthLost;
        else
            robotHealthImage.fillAmount = 0;
    }

    public void SetTempRectangle(float temp)
    {
        if (1f - temp >= 0.02)
            tempRectangleImage.fillAmount = 1f - temp;
        else
            tempRectangleImage.fillAmount = 0;
    }

    public void SetTempCircle(float temp)
    {
        if (1f - temp >= 0.02)
            tempCircleImage.fillAmount = 1f - temp;
        else
            tempCircleImage.fillAmount = 0;
    }

    public void UpdateTempDisplay(int elapsedTime, float firstPhaseDuration, float secondPhaseDuration) 
    {
        // update the first fill image for the first phase duration (temp)
        if (elapsedTime <= firstPhaseDuration)
            SetTempRectangle(elapsedTime / firstPhaseDuration);

        // update the second image for the second phase duration (temp)
        else
        {
            float secondPhaseElapsed = elapsedTime - firstPhaseDuration;
            SetTempCircle(secondPhaseElapsed / secondPhaseDuration);
        }
    }

    public void UpdateBatteryDisplay(float decreaseRate) 
    {
        batteryImage.fillAmount -= decreaseRate * Time.deltaTime;
        if (batteryImage.fillAmount < 0) { batteryImage.fillAmount = 0; }
        UpdateBatteryCooldown();
    }

    public void UpdateBatteryCooldown()
    {
        if (batteryImage.fillAmount <= 0)
            isBatteryOnCooldown = true;
        else if (batteryImage.fillAmount >= batteryCooldownLimit)
            isBatteryOnCooldown = false;
    }

    private string GetFormattedTime(int timeToDisplay) 
    {
        // convert the total time left into minutes and seconds
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    public void UpdateStopwatchDisplay(int time) { stopwatchText.text = GetFormattedTime(time); }

    public void UpdateCurrentObjectiveDisplay(string text) { currObjText.text = text; }

    public void ResetUIValues() 
    {
        SetTempCircle(0f);
        SetTempRectangle(0f);
        UpdateRobotHealthDisplay(-1f);
        deliveryTimer.StopTimer();
        deliveryTimer.ResetTimer();
        UpdateStopwatchDisplay(0);
    }

    private void SetScoreDescription(Color s1, Color s2, Color s3, string text)
    {
        star1.color = s1;
        star2.color = s2;
        star3.color = s3;
        scoreDescText.text = text;
    }

    public void UpdateScoreScreenInfo()
    {
        // get the needed variables for comparison
        int time = deliveryTimer.GetElapsedTime();
        float tempDuration = deliveryTimer.GetTimerDuration();
        float temp = deliveryTimer.GetCurrentTemp() / tempDuration;
        float health = robotHealthImage.fillAmount;
        
        // update the total time taken and description texts
        elapsedTimeText.text = GetFormattedTime(time);   
        scoreTitleText.text = "Delivery Complete!";

        // user receives 3 stars for perfect health and at least half the temp gauge left full
        if (health == 1 && temp >= 0.5) 
            SetScoreDescription(Color.white, Color.black, Color.black, "Excellent Delivery!");

        // user receives 2 stars for perfect health and poor temp, or good temp and decent health
        else if ((health == 1 && temp < 0.5 && temp > 0) || temp >= 0.5 && health >= 0.5)
            SetScoreDescription(Color.white, Color.black, Color.black, "Good Delivery!");

        // user receives 1 star for poor health and poor temp, or no temp and perfect health
        else if (health <= 0.5 && health > 0 && temp < 0.5 && temp > 0)
            SetScoreDescription(Color.white, Color.black, Color.black, "Poor Delivery!");

        // user receives no stars for anything worse (a failed delivery)
        else 
            SetScoreDescription(Color.black, Color.black, Color.black, "temp_string");
            scoreTitleText.text = "Delivery Failed!";

            if (temp <= 0.01 && health > 0)
                scoreDescText.text = "Your food is too cold!";
            else if (health <= 0 && temp > 0.1f)
                scoreDescText.text = "Your robot has been badly damaged!";
            else 
                scoreDescText.text = "Your robot has been badly damaged and your food is cold!";
    }

    public bool HasPlayerLost() { return robotHealthImage.fillAmount == 0 || tempCircleImage.fillAmount <= 0; }

    public void UpdateArrowDestination()
    {
        targetWaypoint = deliveryHandler.GetCurrentWaypoint();
        if (player.transform != null && targetWaypoint != null)
        {
            // Get the direction from the player to the target
            Vector3 direction = targetWaypoint.position - player.transform.position;

            // Calculate the angle to the target
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Smoothly rotate the arrow towards the target
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            arrowImage.rotation = Quaternion.Slerp(arrowImage.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void Update() {
        // UpdateArrowDestination();
    }
}
