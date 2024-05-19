using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;

    [SerializeField] private GameObject pausePanel, gameplayPanel;

    [SerializeField] private TMPro.TextMeshProUGUI currObjText, objTitleText;

    private bool isPaused, canPause = true;


    private void Awake() {
        
        // create a singleton instance of gameplay manager to exist during this execution
        if (instance != null && instance != this) { 
            Destroy(this); 
        } else { 
            instance = this; 
        } 
    }

    private void Start() {
        //Time.timeScale = 0f;
        canPause = true;
        isPaused = false;
    }

    private void Update() {
        // pause the game if ESC is hit by the user
        if (Input.GetKeyDown(KeyCode.Escape))
            pauseGame();
    }

    public void UpdateStartingVariables() {
        Time.timeScale = 1f;
        canPause = true;
        gameplayPanel.SetActive(true);
    }

    public void pauseGame() {
        if (!isPaused && canPause) {
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

    private IEnumerator PlayClickSoundWait() {
        //SoundManager.instance.PlayButtonClick();
        yield return new WaitForSeconds(1f);
    }

    public void MainMenu() {
        PlayClickSoundWait();
        SceneManager.LoadScene(TagManager.MAIN_MENU_SCENE_TAG);
        Time.timeScale = 1f;
    }

    public void restartGame() {
        PlayClickSoundWait();
        gameplayPanel.SetActive(true);
        SceneManager.LoadScene(TagManager.GAMEPLAY_SCENE_TAG);
        Time.timeScale = 1f;
    }

    public void DisplayCurrentObjective(string text)
    {
        currObjText.text = text;
        // objTitleText.text = text2;
    }


    // public void TurnOffInstructions() {
    //     SoundManager.instance.PlayButtonClick();
    //     instructionsPanel.SetActive(false);
    // }

    // public void gameOver() {
    //     Time.timeScale = 0f;
    //     gameOverPanel.SetActive(true);
    //     canPause = false;

    //     // update the score for the end screen
    //     ScoreManager.instance.UpdateEndScoreScreen();
    // }
}
