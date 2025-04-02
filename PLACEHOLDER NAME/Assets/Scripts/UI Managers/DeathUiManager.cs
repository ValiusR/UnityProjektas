using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathUiManager : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI scoreText;
    [SerializeField] public TextMeshProUGUI timeText;

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
        LoadMainMenuScene();
    }

    public virtual void ShowDeathUI()
    {
        if (gameObject != null && this != null)
        {
            scoreText.text = "YOUR SCORE: " + ScoreManager.currScore;
            timeText.text = "YOU HAVE SURVIVED: " + TimerManager.formattedTime;
            Time.timeScale = 0f;
            gameObject.SetActive(true);
        }
    }

    protected virtual void LoadMainMenuScene()
    {
        if(this != null)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}