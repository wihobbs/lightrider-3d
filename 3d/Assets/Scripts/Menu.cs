using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public AudioSource btnClickSound;
    public AudioSource btnClickInvalidSound;

    public void playBtnClickSound(){
        this.btnClickSound.Play();
    }
    public void playInvalidBtnClickSound(){
        this.btnClickSound.Play();
    }
    public void PlayNewGame(){
        // play new game
        // timer to zero
        this.playBtnClickSound();
        Timer.timeValue = Timer.startTimeValue;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ResumeGame(){
        if(SaveSystem.TimeSaved()){
            // there was a time saved
            // resume old game
            this.playBtnClickSound();
            TimerData data = SaveSystem.LoadTime();
            // update static value of class
            Timer.updateTime(data.timeValue);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }else{
            Debug.Log("Menu.cs: No data found.");
            this.btnClickInvalidSound.Play();
        }
    }

    public void QuitGame(){
        Application.Quit();
    }
}
