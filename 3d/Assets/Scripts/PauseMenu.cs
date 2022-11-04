using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public Timer timer;

    public List<GameObject> playerObjects = new List<GameObject>();

    public GameObject pauseMenuUI;

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
        foreach(GameObject player in this.playerObjects){
            player.GetComponent<LightBike>().Save();
        }
    }
}
