using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;

    [SerializeField] private GameObject pausePanel, gameplayPanel, scorePanel;

    public bool isPaused, canPause;


    private void Awake() {
        // create a singleton instance of gameplay manager to exist during this execution
        if (instance != null && instance != this)
            Destroy(this); 
        else 
            instance = this; 
    }

    private void Start() {
        canPause = true;
        isPaused = false;
    }

    private void Update() {
        // pause the game if ESC is hit by the user
        if (Input.GetKeyDown(KeyCode.Escape) /*|| Input.GetButton()*/ && canPause)
            pauseGame();

        // check player's health and temperature values for lose conditions
        if (UIManager.instance.HasPlayerLost())
            DisplayScoreScreen();
    }

    public void pauseGame() {
        if (!isPaused) {
            //SoundManager.instance.PlayButtonClick();
            Time.timeScale = 0f;
            gameplayPanel.SetActive(false);
            pausePanel.SetActive(true);
            isPaused = true;
        } else {
            resumeGame();
        }
    }

    public void resumeGame() {
        //SoundManager.instance.PlayButtonClick();
        gameplayPanel.SetActive(true);
        pausePanel.SetActive(false); 
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void ResumeGameFromScoreScreen() {
        //SoundManager.instance.PlayButtonClick();
        scorePanel.SetActive(false);
        gameplayPanel.SetActive(true);
        Time.timeScale = 1f;
        canPause = true;
    }

    private IEnumerator PlayClickSoundWait() {
        //SoundManager.instance.PlayButtonClick();
        yield return new WaitForSeconds(1f);
    }

    public void MainMenu() {
        PlayClickSoundWait();
        SceneManager.LoadScene(TagManager.TITLE_SCREEN_SCENE_TAG);
        Time.timeScale = 1f;
    }

    public void restartGame() {
        PlayClickSoundWait();
        gameplayPanel.SetActive(true);
        SceneManager.LoadScene(TagManager.GAMEPLAY_SCENE_TAG);
        Time.timeScale = 1f;
    }

    public void DisplayScoreScreen()
    {
        Time.timeScale = 0f;
        scorePanel.SetActive(true);
        gameplayPanel.SetActive(false);
        canPause = false;
        UIManager.instance.UpdateScoreScreenInfo();
        UIManager.instance.ResetUIValues();
    }

    

    // public void TurnOffInstructions() {
    //     SoundManager.instance.PlayButtonClick();
    //     instructionsPanel.SetActive(false);
    // }
}
