using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Levels to Load")]
    public  string _newGameLevel;
    private string levelToLoad;
    public void NewGameDialogYes(){
        SceneManager.LoadScene(_newGameLevel);
    }
    public void ExitGameDialog(){
        Application.Quit();
    }

}
