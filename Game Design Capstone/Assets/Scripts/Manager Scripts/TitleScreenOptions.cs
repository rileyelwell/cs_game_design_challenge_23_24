using UnityEngine;
using UnityEngine.SceneManagement; 

public class TitleScreenOptions : MonoBehaviour
{
    /*
     * Name: PlayGame
     * Inputs: none
     * Outputs: none
     * Description: Starts the game
     */
    public void PlayGame()
    {
        SceneManager.LoadScene(TagManager.GAMEPLAY_SCENE_TAG);
        Time.timeScale = 1f;
    }

    /*
     * Name: QuitGame
     * Inputs: none
     * Outputs: none
     * Description: Quits the game
     */
    public void QuitGame()
    {
        // If we are running in a standalone build of the game
        #if UNITY_STANDALONE
            // Quit the application
            Application.Quit();
        #endif

        // If we are running in the editor
        #if UNITY_EDITOR
            // Stop playing the scene
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
