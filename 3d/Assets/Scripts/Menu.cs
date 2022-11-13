using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public AudioSource btnClickSound;
    public AudioSource btnClickInvalidSound;
    public GameObject NoSaveDataPresentText;

    // bad design, but fast fix
    private bool toasting = false;

    public void SetInactiveNoSaveDataPresent(){
        this.NoSaveDataPresentText.SetActive(false);
        this.toasting = false;
    }

    public void ShowToastNoSaveDataPresent(){
        this.NoSaveDataPresentText.SetActive(true);
        if(!this.toasting){
            // toast
            Invoke("SetInactiveNoSaveDataPresent",2.5f);
        }
        this.toasting = true;
    }
    


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
        // not loading from save
        SaveSystem.LOAD_FROM_SAVE = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ResumeGame(){
        if(SaveSystem.TimeSaved()){
            // loading from save
            SaveSystem.LOAD_FROM_SAVE = true;
            // there was a time saved
            // resume old game
            this.playBtnClickSound();
            TimerData data = SaveSystem.LoadTime();
            // update static value of class
            PauseMenu.updateTime(data.timeValue);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }else{
            Debug.Log("Menu.cs: No data found.");
            ShowToastNoSaveDataPresent();
            this.btnClickInvalidSound.Play();
        }
    }

    public void QuitGame(){
        Application.Quit();
    }
}
