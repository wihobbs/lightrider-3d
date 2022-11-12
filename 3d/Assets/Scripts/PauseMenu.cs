using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public Timer timer;

    public GameObject pauseMenuUI;

    public AudioSource btnClickSound;

    public GameObject SavedDataText;

    // bad design, but fast fix
    private bool toasting = false;

    public void playBtnClickSound(){
        this.btnClickSound.Play();
    }

    public void SetInactiveSavedData(){
        this.SavedDataText.SetActive(false);
        this.toasting = false;
    }
    
    public void ShowToastSavedData(){
        this.SavedDataText.SetActive(true);
        if(!this.toasting){
            Invoke("SetInactiveSavedData",2.5f);
        }
        this.toasting = true;
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(GameIsPaused){
                Resume();
            }
            else{
                Pause();
            }
        }
    }

    public void Resume(){
        // hide menu
        pauseMenuUI.SetActive(false);
        // freeze time
        Time.timeScale = 1f;
        // set paused
        GameIsPaused = false;
    }
    void Pause(){
        // show menu
        pauseMenuUI.SetActive(true);
        // freeze time
        Time.timeScale = 0f;
        // set paused
        GameIsPaused = true;
    }
    public void LoadMainMenu(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
    public void Quit(){
        Application.Quit();
    }

    public void Save(){
        SaveSystem.SaveTime();
        this.ShowToastSavedData();
    }
}
