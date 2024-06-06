using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;                                         // Current instance of the gameplay manager
    [SerializeField] private GameObject pausePanel;                                 // Pause menu
    [SerializeField] private GameObject gameplayPanel;                              // Game UI
    [SerializeField] private GameObject scorePanel;                                 // Score screen
    [SerializeField] private GameObject instructionsPanel;                          // Instructions UI
    [SerializeField] private GameObject customizerPanel;                            // Customizer UI Panel
    [HideInInspector] public bool isPaused, canPause, isDisplayingInstructions;     // Bool states
    [SerializeField] private GameObject[] instructionSections;                      // Instruction Sections
    private int currentSectionIndex = 0;                                            // current index for displaying Instructions
    [SerializeField] private float timeToDisplayInstructions = 3.5f;                // time delay for displaying instructions at start
    [SerializeField] private GameObject playerSticker, playerTopper;
    [SerializeField] private GameObject stickerSelector, topperSelector;
    private CustomizerScreen stickerSelect, topperSelect;
    [SerializeField] private MenuNavigationCustom menuNavCustomScript;

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
        stickerSelect = stickerSelector.GetComponent<CustomizerScreen>();
        topperSelect = topperSelector.GetComponent<CustomizerScreen>();

        canPause = false;
        isPaused = false;

        DisplayCustomizerScreen();
    }

    IEnumerator DisplayInstructions()
    {
        Time.timeScale = 1f;
        customizerPanel.SetActive(false);

        // wait for a couple of frames before calling the function
        yield return new WaitForSeconds(timeToDisplayInstructions);

        Time.timeScale = 0f;

        // initialize all sections to be inactive except the first one
        for (int i = 0; i < instructionSections.Length; i++)
            instructionSections[i].SetActive(i == currentSectionIndex);

        instructionsPanel.SetActive(true);
        gameplayPanel.SetActive(true);

        isDisplayingInstructions = true;
    }

    /*
     * Name: Update (Unity)
     * Inputs: none
     * Outputs: none
     * Description: Checks for pause input or game loss
     */
    private void Update() {
        // don't allow other inputs or checking while displaying instructions at beginning
        if (customizerPanel.activeInHierarchy && Input.GetButtonDown("Submit"))
        {
            SetPlayerCustomization();
            StartCoroutine(DisplayInstructions());
        }
        else if (isDisplayingInstructions && Input.GetButtonDown("Continue")) 
            ShowNextInstruction();
        else 
        {
            // pause the game if ESC is hit by the user
            if (Input.GetButtonDown("Pause") && canPause)
                PauseGame();

            // check player's health and temperature values for lose conditions
            if (UIManager.instance.HasPlayerLost())
                DisplayScoreScreen();
        }
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

    private void ShowNextInstruction() 
    {
        // hide the current section
        instructionSections[currentSectionIndex].SetActive(false);

        // move to the next section
        currentSectionIndex++;

        // if the current section index exceeds the number of sections, end showing instructions
        if (currentSectionIndex >= instructionSections.Length)
        {
            isDisplayingInstructions = false;
            instructionsPanel.SetActive(false);
            Time.timeScale = 1f;
            canPause = true;
            return;
        }
            
        // show the next section
        instructionSections[currentSectionIndex].SetActive(true);
    }

    private void DisplayCustomizerScreen()
    {
        Time.timeScale = 0f;
        customizerPanel.SetActive(true);
    }

    private void SetPlayerCustomization()
    {
        string lastSelectedButton = menuNavCustomScript.GetSelectedButtonName();
        int stickerCurrentIndex = stickerSelect.GetCurrentIndex(0);
        int topperCurrentIndex = topperSelect.GetCurrentIndex(0);

        if (lastSelectedButton == "StickerLeftArrow")
        {
            print("leftStick");
            stickerCurrentIndex = stickerSelect.GetCurrentIndex(1);
            print(stickerCurrentIndex);
        }
            
        else if (lastSelectedButton == "StickerRightArrow")
        {
            print("rightStick");
            stickerCurrentIndex = stickerSelect.GetCurrentIndex(-1);
            print(stickerCurrentIndex);
        }
            
        else if (lastSelectedButton == "TopperLeftArrow")
        {
            print("lefttop");
            topperCurrentIndex = topperSelect.GetCurrentIndex(1);
            print(topperCurrentIndex);
        }
            
        else if (lastSelectedButton == "TopperRightArrow")
        {
            print("righttop");
            topperCurrentIndex = topperSelect.GetCurrentIndex(-1);
            print(topperCurrentIndex);
        }
            

        for (int index = 0; index < playerSticker.transform.childCount; index++)
        {
            // Get the child GameObject at the current index
            GameObject child = playerSticker.transform.GetChild(index).gameObject;

            

            // Check if this is the child we want to keep active
            if (index == stickerCurrentIndex)
                child.SetActive(true);
            else
                child.SetActive(false);
        }

        for (int index = 0; index < playerTopper.transform.childCount; index++)
        {
            // Get the child GameObject at the current index
            GameObject child = playerTopper.transform.GetChild(index).gameObject;

            // Check if this is the child we want to keep active
            if (index == topperCurrentIndex)
                child.SetActive(true);
            else
                child.SetActive(false);
        }
    }
}
