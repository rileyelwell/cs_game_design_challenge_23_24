using UnityEngine;
using UnityEngine.SceneManagement; 

public class TitleScreenOptions : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(TagManager.GAMEPLAY_SCENE_TAG);
        Time.timeScale = 1f;
    }

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
