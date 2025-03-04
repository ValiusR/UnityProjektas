using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuListener : MonoBehaviour
{
    public string pauseMenuSceneName = "PauseMenu";
    private bool isPaused = false;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape has benn pressed");
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    public void PauseGame()
    {
        Debug.Log("PauseGame()");
        isPaused = true;
        Time.timeScale = 0f;
        SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
        
    }
    public void ResumeGame()
    {
        Debug.Log("ResumeGame()");
        isPaused = false;
        Time.timeScale = 1f;
        SceneManager.UnloadSceneAsync("PauseMenu");
    }
}
