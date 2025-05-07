using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{


    [SerializeField] GameObject optionsMenu; // Assign your Options UI Prefab in the Inspector

    public void NewGame()
    {
        Debug.Log("MainMenuManager NewGame()");
        ScoreManager.currScore = 0;
        //LevelUpSystem.experience = 0;
        SceneManager.LoadScene("Main");
        //SceneManager.UnloadSceneAsync("PauseMenu");
    }
    public void Options()
    {
        optionsMenu.SetActive(true);
        Debug.Log("MainMenuManager Options()");
    }

    public void QuitGame()
    {
        Debug.Log("MainMenuManager QuitGame()");
        Application.Quit();
    }
}
