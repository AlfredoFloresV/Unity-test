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


public class MainMenuController2 : MonoBehaviour
{
    [Header("Levels to Load")]
    public string _newGameLevel;
    public string _sceneCredits;
    public string _sceneCollectables;
    private string levelToLoad;
    public TextMeshProUGUI tittle;
    private bool startgame;

    [Header("Sounds")]
    public AudioSource audioSource;

    [Header("Music Controller")]
    public GameObject musicController;

    [SerializeField]
    private GameObject fade;

    private void Start()
    {
        startgame = false;   
        Cursor.visible = true;

    }

    void Update()
    {
        //Debug.Log("audiosource playing:" + audioSource.isPlaying.ToString());
        //Debug.Log("startgame:" + startgame.ToString());
        if (!audioSource.isPlaying && startgame)
        {
            Debug.Log("Load");
            fade.GetComponent<Animator>().Play("FadeOut");
            StartCoroutine(nextScene());
        }
    }

    IEnumerator nextScene() 
    {
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(_newGameLevel);
    }

    public void NewGameDialogYes(){
        Debug.Log("Click");
        Destroy(musicController);
        Debug.Log("AfterDestroy");
        audioSource.Play();
        startgame = true;        
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
    }

    public void changeTextTittle(string textTittle){
        this.tittle.text = textTittle;
    }

}
