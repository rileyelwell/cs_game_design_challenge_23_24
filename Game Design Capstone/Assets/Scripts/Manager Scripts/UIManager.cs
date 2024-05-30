using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;                                                               // Current instance of the gameplay manager
    private DeliveryTimer deliveryTimer;                                                            // Reference to deliverTimer
    private DeliveryHandler deliveryHandler;                                                        // Reference to deliveryHandler
    private Transform player;                                                                       // Reference to player
    [SerializeField] private Image leftSensorImg, rightSensorImg, backSensorImg, frontSensorImg;    // Sensor images
    [SerializeField] private Image robotHealthImage;                                                // Robot health image
    [SerializeField] private Image tempRectangleImage, tempCircleImage;                             // Temperature images
    [SerializeField] private Image batteryImage;                                                    // Battery images
    [SerializeField] private float batteryCooldownLimit = 0.5f;                                     // Battery cooldown
    public bool isBatteryOnCooldown;                                                                // Boolean for the battery on cooldown
    [SerializeField] private TMPro.TextMeshProUGUI stopwatchText;                                   // Timer text
    [SerializeField] private TMPro.TextMeshProUGUI currObjText;                                     // Objective text
    [SerializeField] private TMPro.TextMeshProUGUI scoreDescText, elapsedTimeText, scoreTitleText;  // Scoreboard text
    [SerializeField] private GameObject resumeButton;                                               // Resume button
    [SerializeField] private Image star1, star2, star3, timerImage;                                 // Scoreboard images
    [SerializeField] private RectTransform arrowImage;                                              // Arrow pointer image
    [SerializeField] private float arrowOffset = 25;                                                // Arrow rotation speed
    private Transform targetWaypoint;                                                               // Target waypoint

    /*
     * Name: Awake (Unity)
     * Inputs: none
     * Outputs: none
     * Description: Initializes UI variables
     */
    private void Awake() {
        // Create a singleton instance of UI manager to exist during this execution
        if (instance != null && instance != this)
            Destroy(this); 
        else 
            instance = this; 

        deliveryTimer = gameObject.GetComponent<DeliveryTimer>();
        deliveryHandler = GameObject.Find("DeliveryHandler").GetComponent<DeliveryHandler>();
        player = GameObject.FindGameObjectWithTag(TagManager.PLAYER_TAG).transform;
        targetWaypoint = deliveryHandler.GetCurrentWaypoint();
    }

    /*
     * Name: UpdateSensorDisplay
     * Inputs: booleans for the front, back, left, and right sensors
     * Outputs: none
     * Description: Updates the 4 sensor system display colors to be red if on and grey if off
     */
    public void UpdateSensorDisplay(bool front, bool back, bool left, bool right) {
        frontSensorImg.color = front ? Color.red : Color.gray;
        backSensorImg.color = back ? Color.red : Color.gray;
        leftSensorImg.color = left ? Color.red : Color.gray;
        rightSensorImg.color = right ? Color.red : Color.gray;
    }

    /* UpdateRobotHealthDisplay
     * Name: (Unity)
     * Inputs: float of the amount of health lost
     * Outputs: none
     * Description: Updates the robot health display bar
     */
    public void UpdateRobotHealthDisplay(float healthLost)
    {
        if (robotHealthImage.fillAmount - healthLost >= 0)
            robotHealthImage.fillAmount -= healthLost;
        else
            robotHealthImage.fillAmount = 0;
    }

    /*
     * Name: SetTempRectangle
     * Inputs: float of the bar decrease rate
     * Outputs: none
     * Description: Updates the temperature display bar
     */
    public void SetTempRectangle(float temp)
    {
        if (1f - temp >= 0.02)
            tempRectangleImage.fillAmount = 1f - temp;
        else
            tempRectangleImage.fillAmount = 0;
    }

    /*
     * Name: SetTempCircle
     * Inputs: float of the circle decrease rate
     * Outputs: none
     * Description: Updates the temperature display circle
     */
    public void SetTempCircle(float temp)
    {
        if (1f - temp >= 0.02)
            tempCircleImage.fillAmount = 1f - temp;
        else
            tempCircleImage.fillAmount = 0;
    }

    /*
     * Name: UpdateTempDisplay
     * Inputs: integer of the elapsed time and float of phase1 and phase2 points
     * Outputs: none
     * Description: Updates the temperature display phase changes
     */
    public void UpdateTempDisplay(int elapsedTime, float firstPhaseDuration, float secondPhaseDuration) 
    {
        // Update the first fill image for the first phase duration (temp)
        if (elapsedTime <= firstPhaseDuration)
            SetTempRectangle(elapsedTime / firstPhaseDuration);

        // Update the second image for the second phase duration (temp)
        else
        {
            float secondPhaseElapsed = elapsedTime - firstPhaseDuration;
            SetTempCircle(secondPhaseElapsed / secondPhaseDuration);
        }
    }

    /*
     * Name: UpdateBatteryDisplay
     * Inputs: float of the bar decrease rate
     * Outputs: none
     * Description: Updates the battery display bar
     */
    public void UpdateBatteryDisplay(float decreaseRate) 
    {
        batteryImage.fillAmount -= decreaseRate * Time.deltaTime;
        if (batteryImage.fillAmount < 0) { batteryImage.fillAmount = 0; }
        UpdateBatteryCooldown();
    }

    /*
     * Name: UpdateBatteryCooldown
     * Inputs: none
     * Outputs: none
     * Description: Updates the battery cooldown
     */
    public void UpdateBatteryCooldown()
    {
        if (batteryImage.fillAmount <= 0)
            isBatteryOnCooldown = true;
        else if (batteryImage.fillAmount >= batteryCooldownLimit)
            isBatteryOnCooldown = false;
    }

    /*
     * Name: GetFormattedTime
     * Inputs: integer of the time in seconds
     * Outputs: string of the time in <minutes>:<seconds>
     * Description: Formats the time into minutes and seconds
     */
    private string GetFormattedTime(int timeToDisplay) 
    {
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    /*
     * Name: UpdateStopwatchDisplay
     * Inputs: integer of the time
     * Outputs: none
     * Description: Updates the timer display time
     */
    public void UpdateStopwatchDisplay(int time)
    {
        stopwatchText.text = GetFormattedTime(time);
    }

    /*
     * Name: UpdateCurrentObjectiveDisplay
     * Inputs: string of the objective text
     * Outputs: none
     * Description: Updates the objective display text
     */
    public void UpdateCurrentObjectiveDisplay(string text)
    {
        currObjText.text = text;
    }

    /*
     * Name: ResetUIValues
     * Inputs: none
     * Outputs: none
     * Description: Resets all UI values
     */
    public void ResetUIValues() 
    {
        SetTempCircle(0f);
        SetTempRectangle(0f);
        UpdateRobotHealthDisplay(-1f);
        deliveryTimer.StopTimer();
        deliveryTimer.ResetTimer();
        UpdateStopwatchDisplay(0);
    }

    /*
     * Name: SetScoreDescription
     * Inputs: color of star1, star2, and star3 and string of the score description
     * Outputs: none
     * Description: Sets star color and score description
     */
    private void SetScoreDescription(Color s1, Color s2, Color s3, string text)
    {
        star1.color = s1;
        star2.color = s2;
        star3.color = s3;
        scoreDescText.text = text;
    }

    /*
     * Name: UpdateScoreScreenInfo
     * Inputs: none
     * Outputs: none
     * Description: Updates the score screen
     */
    public void UpdateScoreScreenInfo()
    {
        // Get the needed variables for comparison
        int time = deliveryTimer.GetElapsedTime();
        float tempDuration = deliveryTimer.GetTimerDuration();
        float temp = deliveryTimer.GetCurrentTemp() / tempDuration;
        float health = robotHealthImage.fillAmount;
        
        // Update the total time taken and description texts
        elapsedTimeText.text = GetFormattedTime(time);   
        scoreTitleText.text = "Delivery Complete!";
        resumeButton.SetActive(true);

        // User receives 3 stars for perfect health and at least half the temp gauge left full
        if (health == 1 && temp >= 0.5) 
            SetScoreDescription(Color.white, Color.white, Color.white, "Excellent Delivery!");

        // User receives 2 stars for perfect health and poor temp, or good temp and decent health
        else if ((health == 1 && temp < 0.5 && temp > 0) || temp >= 0.5 && health >= 0.5)
            SetScoreDescription(Color.white, Color.white, Color.black, "Good Delivery!");

        // User receives 1 star for poor health and poor temp, or no temp and perfect health
        else if (health <= 0.5 && health > 0 && temp < 0.5 && temp > 0)
            SetScoreDescription(Color.white, Color.black, Color.black, "Poor Delivery!");

        // User receives no stars for anything worse (a failed delivery)
        else 
            SetScoreDescription(Color.black, Color.black, Color.black, "temp_string");
            scoreTitleText.text = "Delivery Failed!";
            resumeButton.SetActive(false);

            if (temp <= 0.01 && health > 0)
                scoreDescText.text = "Your food is too cold!";
            else if (health <= 0 && temp > 0.1f)
                scoreDescText.text = "Your robot has been badly damaged!";
            else 
                scoreDescText.text = "Your robot has been badly damaged and your food is cold!";
    }

    /*
     * Name: HasPlayerLost
     * Inputs: none
     * Outputs: boolean of if the player lost
     * Description: Checks if the player lost
     */
    public bool HasPlayerLost()
    {
        return robotHealthImage.fillAmount == 0 || tempCircleImage.fillAmount <= 0;
    }

    /*
     * Name: UpdateArrowDesination
     * Inputs: none
     * Outputs: none
     * Description: Update the arrow to point towards goal
     */
    private void UpdateArrowDestination()
    {
        Vector3 directionToGoal = (targetWaypoint.position - player.position).normalized;
        Vector3 playerForward = player.forward;

        float dotProduct = Vector3.Dot(playerForward, directionToGoal);
        float angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg; // Convert the dot product to an angle in degrees

        // Determine the sign of the angle to know which direction to rotate the arrow
        Vector3 crossProduct = Vector3.Cross(playerForward, directionToGoal);
        if (crossProduct.y < 0)
            angle = -angle;

        // Rotate the arrow UI to point towards the goal
        arrowImage.localRotation = Quaternion.Euler(0, 0, angle + arrowOffset);
    }

    private void Update() 
    {
        UpdateArrowDestination();  
    }
}
