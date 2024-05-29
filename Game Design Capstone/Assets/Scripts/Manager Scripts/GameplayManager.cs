using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;             // Current instance of the gameplay manager
    [SerializeField] private GameObject pausePanel;     // Pause menu
    [SerializeField] private GameObject gameplayPanel;  // Game UI
    [SerializeField] private GameObject scorePanel;     // Score screen
    public bool isPaused, canPause;                     // Pause state

    /*
     * Name: Awake (Unity)
     * Inputs: none
     * Outputs: none
     * Description: Creates a singleton instance of gameplay manager to exist during this execution
     */
    private void Awake() {
        if (instance != null && instance != this)
            Destroy(this); 
        else 
            instance = this; 
    }

    /*
     * Name: Start (Unity)
     * Inputs: none
     * Outputs: none
     * Description: Initializes pause state
     */
    private void Start() {
        canPause = true;
        isPaused = false;
    }

    /*
     * Name: Update (Unity)
     * Inputs: none
     * Outputs: none
     * Description: Checks for pause input or game loss
     */
    private void Update() {
        // Pause the game if ESC is hit by the user
        if (Input.GetKeyDown(KeyCode.Escape) /*|| Input.GetButton()*/ && canPause)
            PauseGame();

        // Check player's health and temperature values for lose conditions
        if (UIManager.instance.HasPlayerLost())
            DisplayScoreScreen();
    }

    /*
     * Name: PauseGame
     * Inputs: none
     * Outputs: none
     * Description: Pauses the game
     */
    public void PauseGame() {
        if (!isPaused) {
            //SoundManager.instance.PlayButtonClick();
            Time.timeScale = 0f;
            gameplayPanel.SetActive(false);
            pausePanel.SetActive(true);
            isPaused = true;
        } else {
            ResumeGame();
        }
    }

    /*
     * Name: ResumeGame
     * Inputs: none
     * Outputs: none
     * Description: Resumes the game
     */
    public void ResumeGame() {
        //SoundManager.instance.PlayButtonClick();
        gameplayPanel.SetActive(true);
        pausePanel.SetActive(false); 
        Time.timeScale = 1f;
        isPaused = false;
    }

    /*
     * Name: ResumeGameFromScoreScreen
     * Inputs: none
     * Outputs: none
     * Description: Resumes the game from the score screen
     */
    public void ResumeGameFromScoreScreen() {
        //SoundManager.instance.PlayButtonClick();
        scorePanel.SetActive(false);
        gameplayPanel.SetActive(true);
        Time.timeScale = 1f;
        canPause = true;
    }

    /*
     * Name: PlayClickSoundWait
     * Inputs: none
     * Outputs: none
     * Description: Waits
     */
    private IEnumerator PlayClickSoundWait() {
        //SoundManager.instance.PlayButtonClick();
        yield return new WaitForSeconds(1f);
    }

    /*
     * Name: MainMenu
     * Inputs: none
     * Outputs: none
     * Description: Goes to the main menu
     */
    public void MainMenu() {
        PlayClickSoundWait();
        SceneManager.LoadScene(TagManager.TITLE_SCREEN_SCENE_TAG);
        Time.timeScale = 1f;
    }

    /*
     * Name: RestartGame
     * Inputs: none
     * Outputs: none
     * Description: Restarts the game
     */
    public void RestartGame() {
        PlayClickSoundWait();
        gameplayPanel.SetActive(true);
        SceneManager.LoadScene(TagManager.GAMEPLAY_SCENE_TAG);
        Time.timeScale = 1f;
    }

    /*
     * Name: DisplayScoreScreen
     * Inputs: none
     * Outputs: none
     * Description: Displays the score screen
     */
    public void DisplayScoreScreen()
    {
        Time.timeScale = 0f;
        scorePanel.SetActive(true);
        gameplayPanel.SetActive(false);
        canPause = false;
        UIManager.instance.UpdateScoreScreenInfo();
        UIManager.instance.ResetUIValues();
    }

    
    /*
    public void TurnOffInstructions() {
        SoundManager.instance.PlayButtonClick();
        instructionsPanel.SetActive(false);
    }
    */
}
