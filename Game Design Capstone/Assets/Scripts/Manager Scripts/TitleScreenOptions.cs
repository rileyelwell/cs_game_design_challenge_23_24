using UnityEngine;
using UnityEngine.SceneManagement; 
using System.Collections;
using UnityEngine.UI;

public class TitleScreenOptions : MonoBehaviour
{
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private float duration = 10.0f;
    [SerializeField] private Image progressBarFill;
    [SerializeField] private RectTransform spriteBig, spriteSmall;
    [SerializeField] private Vector2 startPointBig, startPointSmall, endPointBig, endPointSmall;

    private string sceneToLoad;

    private IEnumerator LoadSceneAsync()
    {
        // Activate the loading panel
        loadingPanel.SetActive(true);

        // Start loading the scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);

        // Prevent the scene from activating immediately
        asyncLoad.allowSceneActivation = false;

        float timeElapsed = 0f;

        // While the scene is still loading
        while (!asyncLoad.isDone)
        {
            // Update the fill amount of the progress bar based on timed loading progress
            float progress = Mathf.Clamp01(timeElapsed / duration);
            progressBarFill.fillAmount = progress;

            // Move the sprite across the screen
            if (timeElapsed < duration)
            {
                spriteBig.anchoredPosition = Vector2.Lerp(startPointBig, endPointBig, timeElapsed / duration);
                spriteSmall.anchoredPosition = Vector2.Lerp(startPointSmall, endPointSmall, timeElapsed / duration);
                timeElapsed += Time.deltaTime;
            }
            else
            {
                spriteBig.anchoredPosition = endPointBig;
                spriteSmall.anchoredPosition = endPointSmall;
            }

            // If the scene has loaded, introduce a slight delay before activating it
            if (asyncLoad.progress >= 0.9f)
            {
                // Ensure the sprite has reached the end before activating the scene
                if (timeElapsed >= duration)
                {
                    yield return new WaitForSeconds(0.5f); // Introduce a small delay before activating the scene
                    asyncLoad.allowSceneActivation = true;
                }
            }

            // Wait for the next frame
            yield return null;
        }
    }

    /*
     * Name: PlayGame
     * Inputs: none
     * Outputs: none
     * Description: Starts the game
     */
    public void PlayGame()
    {
        sceneToLoad = TagManager.GAMEPLAY_SCENE_TAG;
        StartCoroutine(LoadSceneAsync());
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
        // if we are running in a standalone build of the game
        #if UNITY_STANDALONE
            Application.Quit();
        #endif

        // if we are running in the editor
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
