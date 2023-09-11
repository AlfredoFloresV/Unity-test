using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/*
    #FFB600
    #FF4A00
    #F50000
*/


public class MainMenuController : MonoBehaviour
{
    [Header("Levels to Load")]
    public string _newGameLevel;
    public string _sceneCredits;
    public string _sceneCollectables;
    private string levelToLoad;
    public TextMeshProUGUI tittle;

    [Header("Sounds")]
    public AudioSource audioSource;
    public AudioClip clip;
    public void NewGameDialogYes(){
        SceneManager.LoadScene(_newGameLevel);
    }
    public void GoToCredits()
    {
        SceneManager.LoadScene(_sceneCredits);
    }
    public void GoToCollectables()
    {
        SceneManager.LoadScene(_sceneCollectables);
    }
    public void ExitGameDialog(){
        Application.Quit();
    }

    public void Click_with_omnius(){
        audioSource.PlayOneShot(clip);
    }

    public void changeTextTittle(string textTittle){
        this.tittle.text = textTittle;
    }

}
