using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [Header("Levels to Load")]
    public string _mainMenu;
    private string levelToLoad;

    static public bool gameIsPaused;

    public GameObject pauseMenuUI;

    void Update(){
        if ( Input.GetKeyDown(KeyCode.Escape)){
            if( gameIsPaused ){ 
                this.ResumePlay();
            }else{
                this.Pause();
            }
        }
    }
    void ResumePlay()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }
    void Pause(){
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
    public void ExitGameDialog()
    {
        SceneManager.LoadScene(_mainMenu);
    }

}
