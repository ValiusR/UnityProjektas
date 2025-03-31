using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{

    [SerializeField] public TextMeshProUGUI timerText;
    public static int seconds = 0;
    public static int minutes = 0;
    public static float elapsedTime;

    public static string formattedTime;


    // Update is called once per frame
    public void Update()
    {
        elapsedTime += Time.deltaTime;
        minutes = Mathf.FloorToInt(elapsedTime / 60);
        seconds = Mathf.FloorToInt(elapsedTime % 60);
        formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);
        if(timerText != null)
        {
            timerText.text = formattedTime;
        }
    }   
}
