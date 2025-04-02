using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuListener : MonoBehaviour
{
    public string pauseMenuSceneName = "PauseMenu";
    public bool isPaused { get; protected set; } = false;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        LoadPauseMenuScene();
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        UnloadPauseMenuScene();
    }

    protected virtual void LoadPauseMenuScene()
    {
        SceneManager.LoadScene(pauseMenuSceneName, LoadSceneMode.Additive);
    }

    protected virtual void UnloadPauseMenuScene()
    {
        SceneManager.UnloadSceneAsync(pauseMenuSceneName);
    }
}