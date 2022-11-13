using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    // starting time of timer
    public static float startTimeValue =5f;
    // elapsed time of timer
    public static float timeValue = startTimeValue; 
    [SerializeField] Text timeText;

    [SerializeField] GameObject timerUi;
    [SerializeField] GameObject timeUpMenuUi;
    [SerializeField] TMP_Text timeUpText;

    [SerializeField] TMP_Text playerOneElimText;
    [SerializeField] TMP_Text playerTwoElimText;

    // tick
    void Update()
    {
        if(timeValue > 0){
            timeValue -= Time.deltaTime; 
            if(timeValue < 5){
                // do something to make the players stressed like change color 
                timeText.color = Color.red;
            }
        }
        else{
            // to do:
            // show timeup
            if(!checkForTie()){
                TimeUp();
            }
            
            // lock the time value to zero
            timeValue = 0;
        }
        DisplayTime(timeValue);
    }

    bool checkForTie(){
        if(int.Parse(this.playerOneElimText.text) != int.Parse(this.playerTwoElimText.text)){
            return false;
        }
        return true;
    }

    void TimeUp(){
        // you can still move, but only at half speed 
        Time.timeScale = 0.5f;
        // hide the timer
        string endText = "GAME OVER\n";
        endText += this.playerOneElimText.text + ":" + this.playerTwoElimText.text + "\n";
        if(int.Parse(this.playerOneElimText.text) > int.Parse(this.playerTwoElimText.text)){
            endText += "PLAYER ONE WINS!";
        }else{
            endText += "PLAYER TWO WINS!";
        }
        this.timeUpText.text = endText;
        timerUi.SetActive(false);
        timeUpMenuUi.SetActive(true);
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
    public static void updateTime(float newTimeValue){
        timeValue = newTimeValue;
    }
}
