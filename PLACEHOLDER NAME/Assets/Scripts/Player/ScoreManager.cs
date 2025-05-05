using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI scoreText;
    public static int currScore = 0;
    public static void addScore(int score)
    {
        currScore += score;
    }

    void Update()
    {
        if(scoreText != null)
        {
            scoreText.text = "Score: " + (currScore + (10 * TimerManager.minutes));
        }
        //Debug.Log("score:" + currScore);
    }

    public void SaveHighScore(int score)
    {
        int currentHighScore = PlayerPrefs.GetInt("HighScore", 0);
        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
        }
    }

    public int LoadHighScore()
    {
        return PlayerPrefs.GetInt("HighScore", 0);
    }

    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
    }
}
