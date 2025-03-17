using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public void NewGame()
    {
        Debug.Log("MainMenuManager NewGame()");
        SceneManager.LoadScene("Main");
        SceneManager.UnloadSceneAsync("PauseMenu");
        /*
        Debug.Log("Resume button: Resume()");
        MenuListener menuListener = FindObjectOfType<MenuListener>();
        if (menuListener != null)
        {
            menuListener.ResumeGame();
        }
       */
    }
    public void Options()
    {
        Debug.Log("MainMenuManager Options()");
    }

    public void QuitGame()
    {
        Debug.Log("MainMenuManager QuitGame()");
        Application.Quit();
    }
}
