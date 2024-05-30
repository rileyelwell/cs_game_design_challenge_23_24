using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    public Image loadingBarFill;
    [SerializeField] float fixedLoadingTime = 10f;

    private static LoadingScreenManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public static LoadingScreenManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LoadingScreenManager>();
                if (instance == null)
                    Debug.LogError("No LoadingScreenManager found in the scene.");
            }
            return instance;
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingScreen.SetActive(true);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        float elapsedTime = 0f;

        while (elapsedTime < fixedLoadingTime)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the progress based on elapsed time
            float progress = Mathf.Clamp01(elapsedTime / fixedLoadingTime);
            if (loadingBarFill != null)
                loadingBarFill.fillAmount = progress;

            yield return null;
        }

        // Ensure the loading bar is filled
        if (loadingBarFill != null)
            loadingBarFill.fillAmount = 1f;

        // Wait until the actual scene is loaded
        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
                asyncOperation.allowSceneActivation = true;

            yield return null;
        }

        loadingScreen.SetActive(false);


        // while (!asyncOperation.isDone)
        // {
        //     float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
        //     if (loadingBarFill != null)
        //         loadingBarFill.fillAmount = progress;

        //     // Activate the scene when loading is complete
        //     if (asyncOperation.progress >= 0.9f)
        //         asyncOperation.allowSceneActivation = true;

        //     yield return null;
        // }

        // loadingScreen.SetActive(false);
    }
}
