using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] public GameObject optionsMenu;
    protected MenuListener menuListener;

    public void Resume()
    {
        var listener = GetMenuListener();
        if (listener != null)
        {
            listener.ResumeGame();
        }
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        LoadMainMenuScene();
    }

    public void QuitGame()
    {
        QuitApplication();
    }

    public void Options()
    {
        if (optionsMenu != null)
        {
            optionsMenu.SetActive(true);
        }
    }

    protected virtual MenuListener GetMenuListener()
    {
        return FindObjectOfType<MenuListener>();
    }

    protected virtual void LoadMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    protected virtual void QuitApplication()
    {
        Application.Quit();
    }
}