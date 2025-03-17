using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{

    public void Resume()
    {
        Debug.Log("Resume button: Resume()");
        MenuListener menuListener = FindObjectOfType<MenuListener>();
        if (menuListener != null)
        {
            menuListener.ResumeGame();
        }
    }
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
