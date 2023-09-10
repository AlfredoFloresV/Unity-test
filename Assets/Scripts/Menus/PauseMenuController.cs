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
    public void ResumePlay()
    {
        Debug.Log("Todo Resume");
    }
    public void ExitGameDialog()
    {
        SceneManager.LoadScene(_mainMenu);
    }

}
