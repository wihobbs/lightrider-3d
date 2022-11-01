using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] float timeValue = 90; // time of timer
    [SerializeField] TMP_Text timeText;

    // tick
    void Update()
    {
        if(timeValue > 0){
            timeValue -= Time.deltaTime; 
        }
        else{
            // to do:
            // end the game
            
            // lock the time value to zero
            timeValue = 0;
        }
        DisplayTime(timeValue);
    }

    void DisplayTime(float timeToDisplay){
        // fix time glitch
        if(timeToDisplay < 0){
            timeToDisplay = 0;
        }
        float minutes = Mathf.FloorToInt(timeToDisplay/60);
        float seconds = Mathf.FloorToInt(timeToDisplay%60);
        float milliseconds = timeToDisplay%1 * 1000;

        timeText.text = string.Format("{0:00}:{1:00}:{2:000}",minutes,seconds,milliseconds);
    }
}
