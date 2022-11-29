using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    // starting time of timer
    // 5 minute rounds
    const float startTimeValue = 5*60f;
    // elapsed time of timer
    public static float timeValue = startTimeValue; 
    [SerializeField] Text timeText;

    [SerializeField] GameObject timerUi;
    [SerializeField] GameObject timeUpMenuUi;
    [SerializeField] TMP_Text timeUpText;

    [SerializeField] TMP_Text playerOneElimText;
    [SerializeField] TMP_Text playerTwoElimText;

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    public AudioSource btnClickSound;
    public AudioSource btnSaveGameSound;

    public GameObject SavedDataText;

    // bad design, but fast fix
    private bool toasting = false;

    private bool gameOver = false;

    [SerializeField] List<GameObject> playerObjects = new List<GameObject>();

    public AudioSource resumeSound;

    [SerializeField] GameObject gameWasResumedText;

    // on start
    void Start(){
        Time.timeScale = 1f;
        if(SaveSystem.LOAD_FROM_SAVE){
            TimerData data = SaveSystem.LoadTime();
            timeValue = data.timeValue;
            this.gameWasResumedText.SetActive(true);
            Invoke("HideResumedGameText",2F);
            // play resume sound
            this.resumeSound.Play();
        }else{
            timeValue = startTimeValue;
        }
    }
    public void HideResumedGameText(){
        this.gameWasResumedText.SetActive(false);
    }

    public void playBtnClickSound(){
        this.btnClickSound.Play();
    }
    public void playBtnSaveGameSound(){
        this.btnSaveGameSound.Play();
    }

    public void SetInactiveSavedData(){
        this.SavedDataText.SetActive(false);
        this.toasting = false;
    }
    
    public void ShowToastSavedData(){
        this.SavedDataText.SetActive(true);
        if(!this.toasting){
            Debug.Log("not toasting");
            Invoke("SetInactiveSavedData",2.5f);
        }
        this.toasting = true;
    }

    void Update(){
        // call timer function
        this.Tick();
        if(!this.gameOver && Input.GetKeyDown(KeyCode.Escape)){
            if(GameIsPaused){

                Resume();
            }
            else{
                Pause();
            }
        }
    }
    void Tick(){
        if(timeValue > 0){
            timeValue -= Time.deltaTime; 
            if(timeValue < 5){
                // do something to make the players stressed like change color 
                timeText.color = Color.red;
            }
            // display time if time present
            this.DisplayTime(timeValue);
        }
        else{
            // no time left
            // show timeup
            if(!this.checkForTie()){
                this.TimeUp();
                SaveSystem.DeleteSave();
                this.gameOver = true;
            }
            // change timer text to "SUDDEN DEATH"
            this.timeText.text = "SUDDEN DEATH";
        }
    }

    public void Resume(){
        // hide menu
        pauseMenuUI.SetActive(false);
        // unfreeze time
        Time.timeScale = 1f;
        // set paused
        GameIsPaused = false;
        // show timer ui
        this.timerUi.SetActive(true);
    }
    void Pause(){
        // show menu
        pauseMenuUI.SetActive(true);
        // freeze time
        Time.timeScale = 0f;
        // set paused
        GameIsPaused = true;
        // hide timer ui
        this.timerUi.SetActive(false);
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
        foreach(GameObject player in this.playerObjects){
            player.GetComponent<LightBike>().Save();
        }
        
        this.ShowToastSavedData();
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
            endText += "BLUE PLAYER WINS!";
        }else{
            endText += "RED PLAYER WINS!";
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
