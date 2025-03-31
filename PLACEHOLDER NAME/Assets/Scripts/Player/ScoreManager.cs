using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    [SerializeField] public TextMeshProUGUI scoreText;
    public static int currScore = 0;
    public static void addScore(int score)
    {
        currScore += score;
    }

    public void Update()
    {
        if(scoreText != null)
        {
            scoreText.text = "Score: " + (currScore + (10 * TimerManager.minutes));
        }
        //Debug.Log("score:" + currScore);
    }


}
