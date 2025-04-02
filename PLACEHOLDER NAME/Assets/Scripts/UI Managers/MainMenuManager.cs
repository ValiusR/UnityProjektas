using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] public GameObject optionsMenu;

    public void NewGame()
    {
        LoadGameScene();
    }

    public void Options()
    {
        if (optionsMenu != null)
        {
            optionsMenu.SetActive(true);
        }
    }

    public void QuitGame()
    {
        QuitApplication();
    }

    protected virtual void LoadGameScene()
    {
        SceneManager.LoadScene("Main");
    }

    protected virtual void QuitApplication()
    {
        Application.Quit();
    }
}