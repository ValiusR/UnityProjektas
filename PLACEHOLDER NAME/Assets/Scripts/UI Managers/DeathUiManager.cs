using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathUiManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI timeText;

    void Start()
    {
        HideUI();
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void QuitToMainMenu()
    {
        this.HideUI();
        SceneManager.LoadScene("MainMenu");
    }

    public virtual void ShowDeathUI()
    {

        //timeText.text = T
        if (gameObject != null)
        {
            scoreText.text = "YOUR SCORE: " + ScoreManager.currScore;
            timeText.text = "YOU HAVE SURVIVED: " + TimerManager.formattedTime;
            Time.timeScale = 0f;
            gameObject.SetActive(true);
        }
        
    }
}
