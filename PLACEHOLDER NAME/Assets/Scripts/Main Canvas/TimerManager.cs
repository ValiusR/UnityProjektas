using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI timerText;
    public static int seconds = 0;
    public static int minutes = 0;
    float elapsedTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        minutes = Mathf.FloorToInt(elapsedTime / 60);
        seconds = Mathf.FloorToInt(elapsedTime % 60);
        //timerText.text = string.Format("{0}:{1:00}:{2:00}", "Timer", minutes, seconds);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }   
}
